using System.Text.RegularExpressions;
using Microsoft.Playwright;
using Microsoft.Playwright.MSTest;

namespace PlaywrightTests;

[TestClass]
public class GuiTest : PageTest
{
    private IPlaywright _playwright;
    private IBrowser _browser;
    private IBrowserContext _browserContext;
    private IPage _page;

    [TestInitialize]
    public async Task Setup()
    {
        _playwright = await Microsoft.Playwright.Playwright.CreateAsync();
        _browser = await _playwright.Chromium.LaunchAsync(
            new BrowserTypeLaunchOptions
            {
                Headless = true, // true = ingen GUI
                // SlowMo = 5, // Lägger in en fördröjning så vi kan se vad som händer
            }
        );
        _browserContext = await _browser.NewContextAsync();
        _page = await _browserContext.NewPageAsync();
    }

    [TestCleanup]
    public async Task Cleanup()
    {
        await _browserContext.CloseAsync();
        await _browser.CloseAsync();
        _playwright.Dispose();
    }

    /* [TestMethod]
     public async Task ManageTicketAsCustomer()
     {
         try
         {
             _page.SetDefaultTimeout(60000);

             // 1. Navigera till sidan
             await _page.GotoAsync("http://localhost:5001/customer-view/7b1d50a0292144bbbce4b1905bf2dcec");
             await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);

             // 2. Vänta in textfältet och fyll i svaret
             var textbox = _page.GetByRole(AriaRole.Textbox, new()
             {
                 NameRegex = new Regex("reply", RegexOptions.IgnoreCase)
             });

             await textbox.WaitForAsync(); // viktig i CI
             await textbox.FillAsync("alright, thanks for the help");

             // 3. Klicka på "Send Reply"
             var sendButton = _page.GetByRole(AriaRole.Button, new() { Name = "Send Reply" });
             await sendButton.ClickAsync();
         }
         catch (Exception ex)
         {
             // Ta en screenshot om det går fel
             var screenshotPath = Path.Combine(Directory.GetCurrentDirectory(), $"error_{DateTime.Now:yyyyMMdd_HHmmss}.png");
             await _page.ScreenshotAsync(new PageScreenshotOptions { Path = screenshotPath });
             Console.WriteLine($"Testet misslyckades. Skärmdump sparad till: {screenshotPath}");
             Console.WriteLine($"Felmeddelande: {ex.Message}");

             // Kasta vidare så att testet fortfarande failar
             throw;
         }
     }*/

    [TestMethod]
    public async Task LoginAdmin()
    {
        await _page.GotoAsync("http://localhost:5001/");
        await _page.GetByRole(AriaRole.Link, new() { Name = "log-in for businesses" }).ClickAsync();
        await _page.GetByRole(AriaRole.Textbox, new() { Name = "Email" }).ClickAsync();
        await _page.GetByRole(AriaRole.Textbox, new() { Name = "Email" }).FillAsync("cj@cj.com");
        await _page.GetByRole(AriaRole.Textbox, new() { Name = "Password" }).ClickAsync();
        await _page.GetByRole(AriaRole.Textbox, new() { Name = "Password" }).FillAsync("1");
        await _page.GetByRole(AriaRole.Button, new() { Name = "Login" }).ClickAsync();
    }

    [TestMethod]
    public async Task NavTesting()
    {
        await _page.GotoAsync("http://localhost:5001/");
        await _page.GetByRole(AriaRole.Link, new() { Name = "log-in for businesses" }).ClickAsync();
        await _page.GetByRole(AriaRole.Textbox, new() { Name = "Email" }).ClickAsync();
        await _page.GetByRole(AriaRole.Textbox, new() { Name = "Email" }).FillAsync("cj@cj.com");
        await _page.GetByRole(AriaRole.Textbox, new() { Name = "Password" }).ClickAsync();
        await _page.GetByRole(AriaRole.Textbox, new() { Name = "Password" }).FillAsync("1");
        await _page.GetByRole(AriaRole.Button, new() { Name = "Login" }).ClickAsync();
        await _page.GetByRole(AriaRole.Link, new() { Name = "Dashboard" }).ClickAsync();
        await _page.GetByRole(AriaRole.Link, new() { Name = "Edit Categories" }).ClickAsync();
        await _page
            .GetByRole(AriaRole.Textbox, new() { Name = "Please enter new category" })
            .ClickAsync();
        await _page
            .GetByRole(AriaRole.Textbox, new() { Name = "Please enter new category" })
            .FillAsync("test");
        await _page.GetByRole(AriaRole.Button, new() { Name = "ADD" }).ClickAsync();
        await _page
            .Locator("div")
            .Filter(new() { HasTextRegex = new Regex("^-test✎$") })
            .GetByRole(AriaRole.Button)
            .Nth(1)
            .ClickAsync();
        await _page.Locator("input[name=\"Cat\"]").ClickAsync();
        await _page.Locator("input[name=\"Cat\"]").FillAsync("testedit");
        await _page.GetByRole(AriaRole.Button, new() { Name = "✓" }).ClickAsync();
        await _page
            .Locator("div")
            .Filter(new() { HasTextRegex = new Regex("^-testedit✎$") })
            .GetByRole(AriaRole.Button)
            .First.ClickAsync();
        await _page.GetByRole(AriaRole.Link, new() { Name = "Tickets" }).ClickAsync();
        await _page.GetByRole(AriaRole.Button, new() { Name = "ID ↕️" }).ClickAsync();
        await _page.GetByRole(AriaRole.Button, new() { Name = "Date ↕️" }).ClickAsync();
        await _page.GetByRole(AriaRole.Link, new() { Name = "Add Questions" }).ClickAsync();
        await _page
    .Locator("div")
    .Filter(new() { HasTextRegex = new Regex("Select Category", RegexOptions.IgnoreCase | RegexOptions.Singleline) })
    .GetByRole(AriaRole.Combobox)
    .First
    .SelectOptionAsync(new[] { "3" });
        await _page.GetByRole(AriaRole.Textbox, new() { Name = "New question" }).ClickAsync();
        await _page.GetByRole(AriaRole.Textbox, new() { Name = "New question" }).FillAsync("test");
        await _page.Locator("select[name=\"category_id\"]").SelectOptionAsync(new[] { "3" });
        await _page.GetByRole(AriaRole.Button, new() { Name = "Add Question" }).ClickAsync();

        await _page.GetByRole(AriaRole.Link, new() { Name = "Manage Employee" }).ClickAsync();
        await _page.GetByRole(AriaRole.Textbox, new() { Name = "enter new employee's name" }).ClickAsync();
        await _page.GetByRole(AriaRole.Textbox, new() { Name = "enter new employee's name" }).FillAsync("Edvin");
        await _page.GetByRole(AriaRole.Textbox, new() { Name = "enter new employee's name" }).PressAsync("Tab");
        await _page.GetByRole(AriaRole.Textbox, new() { Name = "enter new employee's email" }).FillAsync("Eddeddsson@gmail.com");
        await _page.GetByRole(AriaRole.Button, new() { Name = "Add employee" }).ClickAsync();
        //await _page
        //.GetByTitle("Eliminate")
        // .First
        // .ClickAsync();
        await _page.Locator("div").Filter(new() { HasTextRegex = new Regex("Reset Password", RegexOptions.IgnoreCase) }).Locator("#ResetPassButton").First.ClickAsync();
    }
    [TestMethod]
    public async Task CreateTicketAsCustomer()
    {
        _page.SetDefaultTimeout(60000);
        await _page.GotoAsync("http://localhost:5001/company/1");
        await _page.GetByRole(AriaRole.Textbox, new() { Name = "Enter your name:" }).ClickAsync();
        await _page.GetByRole(AriaRole.Textbox, new() { Name = "Enter your name:" }).FillAsync("Edvin");
        await _page.GetByRole(AriaRole.Textbox, new() { Name = "Enter your email:" }).ClickAsync();
        await _page.GetByRole(AriaRole.Textbox, new() { Name = "Enter your email:" }).FillAsync("Eddeddsson@gmail.com");
        await _page.GetByRole(AriaRole.Textbox, new() { Name = "Enter your Message:" }).ClickAsync();
        await _page.GetByRole(AriaRole.Textbox, new() { Name = "Enter your Message:" }).FillAsync("I need help");
        await _page.Locator("div").Filter(new() { HasTextRegex = new Regex("Choose a category") }).GetByRole(AriaRole.Combobox).SelectOptionAsync(new[] { "15" });
        await _page.GetByRole(AriaRole.Button, new() { Name = "Submit" }).ClickAsync();
    }
    [TestMethod]
    public async Task ManageTicketAsEmployee()
    {
        await _page.GotoAsync("http://localhost:5001/");
        await _page.GetByRole(AriaRole.Link, new() { Name = "log-in for businesses" }).ClickAsync();
        await _page.GetByRole(AriaRole.Textbox, new() { Name = "Email" }).ClickAsync();
        await _page.GetByRole(AriaRole.Textbox, new() { Name = "Email" }).FillAsync("cj@cj.com");
        await _page.GetByRole(AriaRole.Textbox, new() { Name = "Password" }).ClickAsync();
        await _page.GetByRole(AriaRole.Textbox, new() { Name = "Password" }).FillAsync("1");
        await _page.GetByRole(AriaRole.Button, new() { Name = "Login" }).ClickAsync();
        await _page.GetByRole(AriaRole.Link, new() { Name = "Tickets" }).ClickAsync();
        await _page.GetByRole(AriaRole.Link, new() { Name = "#11" }).ClickAsync();
        await _page.GetByRole(AriaRole.Textbox, new() { Name = "Reply ... (signature will be" }).ClickAsync();
        await _page.GetByRole(AriaRole.Textbox, new() { Name = "Reply ... (signature will be" }).FillAsync("this will ship in 2 days");
        await _page.GetByRole(AriaRole.Button, new() { Name = "Send Reply" }).ClickAsync();
    }

    [TestMethod]
    public async Task SolveTicketAsEmployee()
    {
        await _page.GotoAsync("http://localhost:5001/");
        await _page.GetByRole(AriaRole.Link, new() { Name = "log-in for businesses" }).ClickAsync();
        await _page.GetByRole(AriaRole.Textbox, new() { Name = "Email" }).ClickAsync();
        await _page.GetByRole(AriaRole.Textbox, new() { Name = "Email" }).FillAsync("cj@cj.com");
        await _page.GetByRole(AriaRole.Textbox, new() { Name = "Password" }).ClickAsync();
        await _page.GetByRole(AriaRole.Textbox, new() { Name = "Password" }).FillAsync("1");
        await _page.GetByRole(AriaRole.Button, new() { Name = "Login" }).ClickAsync();
        await _page.GetByRole(AriaRole.Link, new() { Name = "Tickets" }).ClickAsync();
        await _page.GetByRole(AriaRole.Link, new() { Name = "#11" }).ClickAsync();
        await _page.GetByRole(AriaRole.Button, new() { Name = "Mark as solved" }).ClickAsync();
    }
}
