using Newtonsoft.Json.Linq;

namespace Angeleo.AffiliateService.Tests.Framework;

public class GraphQlResponse
{
    public JObject[] errors { get; set; }
    public JObject data { get; set; }

    public bool ContainsErrors()
    {
        return errors != null && errors.Any();
    }

    public bool ContainErrorWithCode(string errorCode)
    {
        return errors != null && errors.Any(e =>
            e["extensions"].Value<JObject>()["code"].Value<string>() == errorCode);
    }

    public bool ContainsAuthorizationError()
    {
        return errors != null && errors.Any(e =>
            e["message"].Value<string>() == "The current user is not authorized to access this resource.");
    }
}