using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace TestProject1
{
    public class AsyncAwait
    {
        private readonly ITestOutputHelper _output;

        public AsyncAwait(ITestOutputHelper output) => 
            _output = output;

        [Fact]
        public Task CreateTask()
        {
            var task = Task<int>.Run(() => 10);
            return task
                .ContinueWith(t => _output.WriteLine(t.Result.ToString()))
                .ContinueWith(t => throw new FileNotFoundException("see if this fails the test"));

        }
        
        [Fact]
        public async Task CreateTaskUsingAwait()
        {
            var result = await Task<int>.Run(() => 10);
            await Task.Run(() => _output.WriteLine(result.ToString()));
            
            throw new FileNotFoundException("see if this fails the test");
        }

        [Fact]
        public async Task ListOfTasks()
        {
            await foreach (var item in Slow())
            {
                _output.WriteLine(item.ToString());
            }
        }

        private async IAsyncEnumerable<int> Slow()
        {
            await Task.Delay(TimeSpan.FromSeconds(4));
            yield return 0;
            
            await Task.Delay(TimeSpan.FromSeconds(1));
            yield return 1;
        }
    }
}