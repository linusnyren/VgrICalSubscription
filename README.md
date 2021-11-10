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

How to subscribe to this on your device depends on your device, here is some quick links.  
[Iphone](https://www.macrumors.com/how-to/subscribe-to-calendars-on-iphone-ipad/)  
[Other devices](https://schulichmeds.com/sites/default/files/Documents/Calendar%20Subscription%20Instructions.pdf)  

## If you just wanna try this code out!!
Run and test this easily with docker compose  
docker-compose up
  
Please note you'll need an login at Heroma självservice(selfservice).

## Tech
This API is built using .Net and C#.  
It uses Selenium to get cookies and tokens.  
It uses an InMemoryCache to ease the amount of calls as a security measure (Not to piss Heroma of).
