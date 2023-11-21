using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PM2E2GRUPO3.Controllers {
    public class VideoRecorder {
        private byte[] videoArray;
        private string localFilePath;



        public VideoRecorder() {}



        public string GetLocalFilePath() {
            return this.localFilePath;
        }



        public async Task<byte[]> GrabarVideo() {
            try {
                if (MediaPicker.Default.IsCaptureSupported) {

                    FileResult video = await MediaPicker.Default.CaptureVideoAsync();

                    using (MemoryStream ms = new MemoryStream()) {
                        Stream st = await video.OpenReadAsync();
                        await st.CopyToAsync(ms);
                        videoArray = ms.ToArray();
                    }

                    // save the file into local storage
                    localFilePath = Path.Combine(FileSystem.CacheDirectory, video.FileName);
                    using (FileStream videoFile = File.OpenWrite(localFilePath)) {
                        Stream st = new MemoryStream(videoArray);
                        await st.CopyToAsync(videoFile);
                    }

                    return videoArray;

                } else {
                    return new byte[0];
                }

            }catch(Exception ex) {
                Console.WriteLine(ex.Message);
                return new byte[0];
            }
        }
    }
}
