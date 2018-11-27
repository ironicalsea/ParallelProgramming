using System;
using System.Collections.Concurrent;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ParallelProgramming
{
    public interface IAsyncSearchProvider
    {
        Task<string> SearchAsync(string text);
    }

    public class AsyncSearchProvider : IAsyncSearchProvider
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private int counter = 0;

        public async Task<string> SearchAsync(string text)
        {
            Interlocked.Increment(ref counter);
            Console.WriteLine($"AsyncSearchProvider #{counter}. search text: {text}");
            
            var page = await 
                _httpClient
                    .GetAsync($"https://ya.ru?q={Uri.EscapeUriString(text)}")
                    .ConfigureAwait(false);
            return await page.Content.ReadAsStringAsync().ConfigureAwait(false);
        }
    }

    public class AsyncCachedSearchProvider : IAsyncSearchProvider
    {
        public AsyncCachedSearchProvider(IAsyncSearchProvider provider)
        {
            _provider = provider;
        }

        public async Task<string> SearchAsync(string text)
        {
            throw new NotImplementedException();
        }

        private readonly IAsyncSearchProvider _provider;

        private readonly ConcurrentDictionary<string, Task<string>> _cache = new ConcurrentDictionary<string, Task<string>>();


    }
}