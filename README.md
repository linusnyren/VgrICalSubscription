# VgrICalSubscription  
## If you just wanna try this out!!
Just download the Dockerfile, the dockerfile clones this repository, sets up selenium with firefox and builds everything, eventually serves it at localhost:5001.   
You can build and run the dockerfile with  
docker build -t foo . && docker run -it -p 5001:5001 foo  
  
Please note you'll need an login at Heroma självservice(selfservice).

## Short description
This api logs in to Heroma to get cookies and access tokens.  
It then uses the cookies and tokens to retrieve an Ical calendar.

It responds to GET calls /schema/user/password/months and returns an ical file which a user can subscribe to on their iPhone or Android device.

This API is built using .Net and C#.  
It uses Selenium to get cookies and tokens.  
It uses an InMemoryCache to ease the amount of calls as a security measure.
