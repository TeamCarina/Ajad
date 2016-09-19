============Bank Application============
Author: Sandeep Modi
Mail: sandeep.modi123@gmail.com
Skype: sandeep.modi123
----------------------------------------

Softwares required
-------------------
1) Visual Studio 2013
2) SQL Server 2008

Steps to setup application
--------------------------
1) Run SQL file in your SQL Server instance and create database from given SQL file. Now you have database of application
2) Open SampleApp.sln file in your visual studio
3) Change database connection strings in SampleApp, SampleApp.Comm projects
4) Then build your application.
5) Run your application
6) You will see two applications are running BankApplication web and ATM desktop application

Note: To use ATM application bank web application is required to run in your browser. because I used WebAPI in ATM application(Not hosted in any server, IIS Express will work)

Sample logins
-------------
Web application has 2 logins
1)	Username: sandeep.modi123@gmail.com
	Password: Welcome1@#
	Account Number: CA0001
	ATM PIN: 1234
2)	Username: sandeep.modi@wipro.com
	Password: Welcome1@#
	Account Number: CA0002
	ATM PIN: 1234

You will get some sample transections in application