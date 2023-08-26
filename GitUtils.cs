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
    }
}
