namespace WorldDomination.DelegatedAuthentication.Auth0
{
    /*
    {
       "email": "i.am.a.princess@rebelalliance.net",
       "email_verified": true,
       "name": "Leia Organa",
       "given_name": "Leia",
       "family_name": "organa",
       "picture": "http://images.techtimes.com/data/images/full/168082/princess-leia-organa.jpg?w=600",
       "gender": "female",
       "locale": "en-GB",
       "clientID": "MYsa7s3Uw75IfNj7H123412341234s",
       "updated_at": "2017-06-01T14:02:21.272Z",
       "user_id": "google-oauth2|11671232005123412341234",
       "nickname": "Princess Leia",
       "identities": [
       {
           "provider": "google-oauth2",
           "user_id": "11671232005123412341234",
           "connection": "google-oauth2",
           "isSocial": true
       }
       ],
       "created_at": "2017-04-23T07:07:46.064Z",
       "app_metadata": {},
       "persistent": {},
       "iss": "https://<your-auth0-tenant>.au.auth0.com/",
       "sub": "google-oauth2|11671232005123412341234",
       "aud": "MYsa7s3Uw75IfNj7H123412341234s",
       "exp": 1496361742,
       "iat": 1496325742
   }
   */
    public class Auth0Jwt : Jwt
    {
        /// <summary>
        /// Full name of authenticated person.
        /// </summary>
        public string Name { get; set; }

        public string Email { get; set; }

        public string Locale { get; set; }

        public string Picture { get; set; }

        public string Gender { get; set; }
    }
}