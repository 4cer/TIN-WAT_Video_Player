using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using WMPLib;

namespace SerwisTIN
{
    // UWAGA: możesz użyć polecenia „Zmień nazwę” w menu „Refaktoryzuj”, aby zmienić nazwę klasy „Service1” w kodzie i pliku konfiguracji.
    public class StreamerService : IStreamerService
    {
        public Stream EchoStream(Stream stream)
        {
            throw new NotImplementedException();
        }

        public List<string> GetFilms()
        {
            //throw new NotImplementedException();
            List<string> files = Directory.GetFiles("D:\\Videos\\vid").ToList<string>();
            List<string> output = new List<string>();
            foreach(string file in files)
            {
                String[] elems = file.Split('\\');
                output.Add(elems[elems.Length - 1]);
            }
            return output;
        }

        public double GetFilmLen(string data)
        {
            double len = 0;

            var player = new WindowsMediaPlayer();
            var clip = player.newMedia("D:\\Videos\\vid\\" + data);
            len = clip.duration;

            return len;
        }

        public Stream GetStreamChunk(string data, int start, int end)
        {
            string filePath = "D:\\Videos\\vid\\" + data;
            try
            {
                FileStream videoFile = File.OpenRead(filePath);
                return videoFile;
            }
            catch (IOException ex)
            {
                Console.WriteLine(
                    String.Format("An exception was thrown while trying to open file {0}", filePath));
                Console.WriteLine("Exception is: ");
                Console.WriteLine(ex.ToString());
                throw ex;
            }
        }

        public Stream GetStream(string data)
        {
            //throw new NotImplementedException();

            //this file path assumes the image is in
            // the Service folder and the service is executing
            // in service/bin
            string filePath = "D:\\Videos\\vid\\" + data;
            //open the file, this could throw an exception
            //(e.g. if the file is not found)
            //having includeExceptionDetailInFaults="True" in config
            // would cause this exception to be returned to the client
            try
            {
                FileStream videoFile = File.OpenRead(filePath);
                return videoFile;
            }
            catch (IOException ex)
            {
                Console.WriteLine(
                    String.Format("An exception was thrown while trying to open file {0}", filePath));
                Console.WriteLine("Exception is: ");
                Console.WriteLine(ex.ToString());
                throw ex;
            }
        }

        public bool UploadStream(Stream stream)
        {
            throw new NotImplementedException();
        }
    }
}
