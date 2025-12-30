using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using System.Net.Http.Headers;

namespace BlogIntegrationTests
{
    public class PostsEndpointsTests : IClassFixture<CustomWebAppFactory>
    {
        private readonly HttpClient _httpClient;

        public PostsEndpointsTests(CustomWebAppFactory factory)
        {
            _httpClient = factory.CreateClient();
        }

        [Fact]
        public async Task GetAllPosts_Authorized_Returns_OK()
        {
            var reg = new { username = "test", email = "t@t.com" , password = "password123" };
            var regResp = await _httpClient.PostAsJsonAsync("/api/Users/Register", reg);
            regResp.EnsureSuccessStatusCode();

            var login = new { username = "test", password = "password123" };
            var loginResp = await _httpClient.PostAsJsonAsync("/api/Users/Login", reg);
            loginResp.EnsureSuccessStatusCode();
            var token = await loginResp.Content.ReadAsStringAsync();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync("/api/Posts");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
