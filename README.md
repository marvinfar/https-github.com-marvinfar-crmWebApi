Firstly you should go to webconfig and change this key to yout Connection: 
  <add key="CrmConnectionString" value="Server=http://1.1.1.1/Organization; Domain=Domain; Username=user; Password=pass;"/>
---
this code developed based on XRM SDKs, so if you encountered by ERROR add the refrences
---
After running the project, you can use it with Postman with the following file content guide which contains cURL (guid Api cURLs.txt)

https://localhost:44381/api/values/{A76FE889-852B-E811-80CC-005056B6C839}

curl --location --globoff 'https://localhost:44381/api/values/{A76FE889-852B-E811-80CC-005056B6C839}' \
--header 'AppName: MyApp' \
--header 'token: 123456' \
--data ''
************************************************

https://localhost:44381/api/values/GetContactByMobile?mobile=09112320258

curl --location 'https://localhost:44381/api/values/GetContactByMobile?mobile=09112320258' \
--header 'AppName: MyApp' \
--header 'token: 123456' \
--data ''
************************************************

https://localhost:44381/api/values/GetContactByName

curl --location --request GET 'https://localhost:44381/api/values/GetContactByName' \
--header 'AppName: MyApp' \
--header 'token: 123456' \
--header 'Content-Type: application/json' \
--data '{
    "firstName" : "محمد حسن",
    "lastName" : "آروین فر"
}'
*******
curl --location --request GET 'https://localhost:44381/api/values/GetContactByName' \
--header 'AppName: MyApp' \
--header 'token: 123456' \
--header 'Content-Type: application/json' \
--data '{
    "firstName" : "",
    "lastName" : "آروین فر"
}'
************************************************

https://localhost:44381/api/values/RegisterContact

curl --location 'https://localhost:44381/api/values/RegisterContact' \
--header 'AppName: MyApp' \
--header 'token: 123456' \
--header 'Content-Type: application/json' \
--data-raw '{
    "firstName" : "John",
    "lastName" : "Doe",
    "emailaddress1" : "john@gmail.com",
    "mobilephone" : "09112320258"
}'
************************************************
https://localhost:44381/api/values/RegisterMultiContacts

curl --location 'https://localhost:44381/api/values/RegisterMultiContacts' \
--header 'AppName: MyApp' \
--header 'token: 123456' \
--header 'Content-Type: application/json' \
--data-raw '[{
    "firstName" : "John",
    "lastName" : "Doe",
    "emailaddress1" : "john@gmail.com",
    "mobilephone" : "09112320258"
},
{
    "firstName" : "Jim",
    "lastName" : "",
    "emailaddress1" : "jim@gmail.com",
    "mobilephone" : "09112320257"
},
{
    "firstName" : "Anna",
    "lastName" : "Hatway",
    "emailaddress1" : "anna@gmail.com",
    "mobilephone" : "09112320256"
}
]'
***********************************************
https://localhost:44381/api/values/UpdateContactByMobile?mobile=09364014489

curl --location --request PUT 'https://localhost:44381/api/values/UpdateContactByMobile?mobile=09112320258' \
--header 'AppName: MyApp' \
--header 'token: 123456' \
--header 'Content-Type: application/json' \
--data-raw '{
    "firstName" : "Name",
    "lastName" : "Family",
    "emailaddress1" : "namefamily@gmail.com",
    "mobilephone" : ""
}'
