# WorldDomination.DelegatedAuthentication

This is a sample application which explores the idea that a DELEGATED AUTHENTICATION service handles the intinial authentication step (i.e. who are you -and- proove it!) while then storing all the details locally in your own database, exlcuding any securty info (which was used to authenticate yourself in the first place).

I think this is a simplified version of an OAuth 2.0 Assertion Flow.

### TL;DR; 

Login with a 3rd party service. They store your security details (password, etc). You only store the boring user's meta data (name, age, email). 
e.g. IdentityServer, Azure Active Directory, Auth0, etc.

No more storing secret information in your system (because, you know .. security is seriously hard) and stop being an attack vector for 'bad-actors' (read: hackers).


### How to get started.

We need to do the following main steps:
- Create some endpoint to accept our JWT token from our Auth service and swap that for our custom JWT which we create.
- Setup JWT bearer middleware, which will respond to _our_ custom JWT tokens.
- Create 2x functions that do the following:
  a) Create a new User/Account if one doesn't exist, else use the existing one.
  b) Copy over any important claims from the source JWT to our new custom one.
- Update the `appsettings.json` file with your Auth0 settings.
- Update your SPA website with the Auth0 settings

### Example steps.
- Create a new ASP.NET Core Web site. e.g. `File -> New -> Project -> Web -> ASP.NET Core Web Application`
- Referece the NuGet package: ``
- Create a custom JWT class, which inherits from `WorldDomination.DelegatedAuthentication.Service.Jwt`.
- Create a new Controller: `AuthenticationController.cs`. This will:
  a) Accept the source JWT
  b) call the `AuthenticationService.Authenticate(..)` method which will create a new custom JWT.
  c) A method that creates a new User or uses an Existing user
  d) A method that copies over any User information into your custom JWT with whatever claims you wish to use.
- Wire up `AddAuthentication` and `AddJwtBearer` in `startup.cs` to handle deserialization of your custom JWT (not the source JWT).


-- end of readme --