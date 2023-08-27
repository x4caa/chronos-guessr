using LibGit2Sharp;
using System.Diagnostics;

namespace chronosguessr
{
    internal class GitUtils
    {
        public static async Task<string> GetFile(string Url)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(Url);
                    response.EnsureSuccessStatusCode();

                    string content = await response.Content.ReadAsStringAsync();
                    return content; // Return the content of the file
                }
                catch (HttpRequestException ex)
                {
                    Debug.WriteLine($"error fetching file content: {ex.Message}");
                    return null; // Return null in case of error
                }
            }
        }

        public static async Task<string[]> GetLines(Task<string> textTask)
        {
            string text = await textTask;
            string[] lines = text.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            return lines;
        }

        public static void CloneRepository(string repoUrl, string localPath)
        {
            try
            {
                CloneOptions options = new CloneOptions();
                //options.IsBare = true;

                Repository.Clone(repoUrl, localPath, options);

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        public static void DeleteDirectory(string target_dir)
        {
            string[] files = Directory.GetFiles(target_dir);
            string[] dirs = Directory.GetDirectories(target_dir);

            foreach (string file in files)
            {
                File.SetAttributes(file, FileAttributes.Normal);
                File.Delete(file);
            }

            foreach (string dir in dirs)
            {
                DeleteDirectory(dir);
            }

            Directory.Delete(target_dir, false);
        }
    }
}
