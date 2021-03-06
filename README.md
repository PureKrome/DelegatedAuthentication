# WorldDomination.DelegatedAuthentication

This is a sample application which explores the idea that a DELEGATED AUTHENTICATION service handles the intinial authentication step (i.e. who are you -and- proove it!) while then storing all the details locally in your own database, exlcuding any securty info (which was used to authenticate yourself in the first place).

I think this is a simplified version of an `OAuth 2.0 Assertion Flow`.

### TL;DR; 

Login with a 3rd party service. They store your security details (password, etc). You only store the boring user's meta data (name, age, email). 
e.g. IdentityServer, Azure Active Directory, Auth0, etc.

No more storing secret information in your system (because, you know .. security is seriously hard) and stop being an attack vector for 'bad-actors' (read: hackers).

=> You still have a `User` database "table" but no username/passwords stored in it.

### What problem is this solving?

- You do *not* store any username/passwords.
- You now use another 3rd party service to do all of this. (Let smart, security people deal with it).
- You still want to store all user info into YOUR database, just not username/passwords.
- You do not want to store OTHER user information information into a 3rd party service because then you cannot use it _easily_ in your own App. (i.e. You want a `User` db table in your db, not in some other 3rd party which you will have to access for any user queries, etc)

### How to get started.

We need to do the following main steps:
- In your webiste, create an endpoint (i.e. `HTTPS POST /authenticate`) which will accept a JWT token from a 3rd party Auth Service (like Auth0, Okta, etc) and swap that for our custom JWT which we create.
- Setup JWT bearer middleware, which will respond to _our_ custom JWT tokens. (This is normal MS middleware)
- Create 2x functions that do the following:
  a) Create a new User/Account if one doesn't exist, else use the existing one. (This library has *no idea* what your db schema/structure/system, is)
  b) Copy over any important claims from the source JWT to our new custom one. (Again, the library has *no idea* what your custom JWT schema/model is and what is important to you. Your custom claims here can include any of your _own_ data and of course, some data from the 3rd Party Auth Service you just used.)
- Update the `appsettings.json` file's 6 keys; 3 from your 3rd party Auth service you are using (e.g. Auth0 ) and 3 for your you (so we can create our own JWT)
- Update your SPA website with the the public 3rd Party settings and your public settings. 

### Example steps.
- Create a new ASP.NET Core Web site. e.g. `File -> New -> Project -> Web -> ASP.NET Core Web Application`
- Referece the NuGet package: `WorldDomination.DelegatedAuthentication`
- Create a custom JWT class, which inherits from `WorldDomination.DelegatedAuthentication.Service.Jwt`.
- Create a new Controller: `AuthenticationController.cs`. This will:
  a) Accept the source JWT (e.g. via a `POST /authenticate`)
  b) Call the ` WorldDomination.DelegatedAuthentication.AuthenticationService.AuthenticateAsync(..)` method which will create your new custom JWT.
  c) A method that creates a new User or uses an Existing user. Again, this library has no concept of what data you wish to store, what your account/user database schema is, etc.
  d) A method that copies over any User information into your custom JWT with whatever claims you wish to use. Again, this library has no idea what User/Account data you wish to store in the final JWT.
- Wire up `AddAuthentication` and `AddJwtBearer` in `startup.cs` to handle deserialization of your custom JWT (not the source JWT).


-- end of readme --