# VgrICalSubscription

This api logs in to Heroma to get cookies and access tokens.  
It then uses the cookies and tokens to retrieve an Ical calendar.

It responds to GET calls /schema/user/password/months and returns an ical file which a user can subscribe to on their iPhone or Android device.

This API is built using .Net and C#.  
It uses Selenium to get cookies and tokens.  
It uses an InMemoryCache to ease the amount of calls as a security measure.
