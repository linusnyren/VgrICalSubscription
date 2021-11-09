# VgrICalSubscription  
## Short description
This api logs in to Heroma to get cookies and access tokens.  
It then uses the cookies and tokens to retrieve an Ical calendar.  
It then serves them as a prenumeration calendar in .ICS format.  

It responds to GET calls /schema/user/password/months and returns an .ICS file which a user can subscribe to on their iPhone or Android device. 

## Usage
Subscribe to this url and fill in your username and password in the web address  
The months parameter lets you specify how many months forward you want to fetch.  
The Username is the username for Heroma, usually the first three letters in your surname and two last in your lastname. (Ann Eklund => annek)  
Password is self explanatory i think...  
https://ec2-34-245-5-138.eu-west-1.compute.amazonaws.com:5001/schema/USERNAME/PASSWORD/MONTHS  

How to subscribe to this on your device depends on your device, here is some quick links.  
[Iphone](https://www.macrumors.com/how-to/subscribe-to-calendars-on-iphone-ipad/)  
[Other devices](https://schulichmeds.com/sites/default/files/Documents/Calendar%20Subscription%20Instructions.pdf)  

## If you just wanna try this code out!!
Just download the Dockerfile, the dockerfile clones this repository, sets up selenium with firefox and builds everything, eventually serves it at localhost:5001.
You need to have a instance of selenium running in a separate docker container.
docker run -d -p 4444:4444 -p 7900:7900 --shm-size="2g" selenium/standalone-firefox
You can build and run the dockerfile with  
docker build -t foo . && docker run -it -p 5001:5001 foo
  
Please note you'll need an login at Heroma självservice(selfservice).

## Tech
This API is built using .Net and C#.  
It uses Selenium to get cookies and tokens.  
It uses an InMemoryCache to ease the amount of calls as a security measure (Not to piss Heroma of).
