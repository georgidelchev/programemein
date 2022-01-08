using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using InstagramApiSharp.API;
using InstagramApiSharp.API.Builder;
using InstagramApiSharp.Classes;
using InstagramApiSharp.Classes.Models;
using InstagramApiSharp.Logger;
using Programemein.Services.Memes;

namespace Programemein.Services.Instagram
{
    public class InstagramService : IInstagramService
    {
        private readonly IMemeService memeService;
        private IInstaApi instaApi;

        public InstagramService(
            IMemeService memeService,
            IInstaApi instaApi)
        {
            this.memeService = memeService;
            this.instaApi = instaApi;
        }

        public async Task<string> UploadAsync()
        {
            try
            {
                var userSession = new UserSessionData
                {
                    UserName = "programemein",
                    Password = "123456"
                };

                var delay = RequestDelay.FromSeconds(2, 2);

                // create new InstaApi instance using Builder
                instaApi = InstaApiBuilder.CreateBuilder()
                    .SetUser(userSession)
                    .UseLogger(new DebugLogger(LogLevel.All)) // use logger for requests and debug messages
                    .SetRequestDelay(delay)
                    .Build();

                const string stateFile = "state.bin";
                try
                {
                    if (File.Exists(stateFile))
                    {
                        Console.WriteLine("Loading state from file");
                        await using var fs = File.OpenRead(stateFile);
                        await instaApi.LoadStateDataFromStreamAsync(fs);
                        // in .net core or uwp apps don't use LoadStateDataFromStream
                        // use this one:
                        // _instaApi.LoadStateDataFromString(new StreamReader(fs).ReadToEnd());
                        // you should pass json string as parameter to this function.
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }

                if (!instaApi.IsUserAuthenticated)
                {
                    // login
                    Console.WriteLine($"Logging in as {userSession.UserName}");
                    delay.Disable();
                    var logInResult = await instaApi.LoginAsync();
                    delay.Enable();
                    if (!logInResult.Succeeded)
                    {
                        Console.WriteLine($"Unable to login: {logInResult.Info.Message}");
                    }
                }

                var state = this.instaApi.GetStateDataAsStream();
                // in .net core or uwp apps don't use GetStateDataAsStream.
                // use this one:
                // var state = _instaApi.GetStateDataAsString();
                // this returns you session as json string.

                await using var fileStream = File.Create(stateFile);
                state.Seek(0, SeekOrigin.Begin);
                await state.CopyToAsync(fileStream);

                var meme = this.memeService
                    .GetOneNonUploadedToInstagramAsByteArray();

                if (meme.IsInInstagram)
                {
                    return null;
                }

                var mediaImage = new InstaImageUpload
                {
                    Height = 1080,
                    Width = 1080,
                    ImageBytes = meme.ImageData.InstagramContent,
                };

                var sb = new StringBuilder();
                foreach (var tag in meme.Tags)
                {
                    sb.AppendLine(tag + " ");
                }

                var result = await this.instaApi
                    .MediaProcessor
                    .UploadPhotoAsync(mediaImage, meme.Title + " " + sb);

                if (result.Succeeded)
                {
                    this.memeService.MarkAsUploadedToInstagram(meme.Id);
                }

                return result.Succeeded
                    ? $"[Successfully] Meme is uploaded: {result.Value.Pk}, {result.Value.Caption}"
                    : $"[ERROR] Meme is not uploaded: {result.Info.Message}";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                // perform that if user needs to logged out
                // var logoutResult = Task.Run(() => _instaApi.LogoutAsync()).GetAwaiter().GetResult();
                // if (logoutResult.Succeeded) Console.WriteLine("Logout succeed");
            }

            return null;
        }
    }
}