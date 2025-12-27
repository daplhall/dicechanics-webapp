#
##Docker run:
```
sudo systemctl start docker
```
## finding ip address
```
docker isnpect <id>
```
http://172.18.0.2:8080/api/Python?program=hello
## The problem
When the program wants to connect we need to ensure that the C# exposed port is mapped to an external port in compose, by defautl c# chooses
8080 os we need to map a port eg 5032 to an out port.

# Angular
```
ng serve --configuration development 
```
for development config

#
I want to create a webapp for Dicechancis akin to troll and anydice.
The language is just gonna be a subset of python. 

So we have a parser that takes then input field parses it through
a python interpreter, gets the results pack and then returns them through an API.

So we have an API that takes a string field of the written code, and returns a json with the statistical data, for the job. 


For the web app it calls this function and has a follwoing interface

This could work with a parser that filteres custom syntax into python
functions

So
+----------------------------------+
| Program         | Visualiztion   |
+-----------------+----------------+
| d = d(6)        | #              |
| g = d8          | # #            |
| show d+g        | # # #          |
|                 | 1 2 3          |
+----------------------------------+

# Attack Plan
Part 1: create a dummy API
1. Create an API that just take input and parses to output

Part 2: Invoking Pyhton
1. Create an object that invokes python
2. Make the python object parse python code
3. Get output from python execution

Part 3: Combine 1 & 2
1. Update api so one can send python code.
2. Update APi so pyhton output get send back as json

Part 4: Adding custom syntax to python input
1. Define new syntax (start with show)
2. make a translation state machine 

Part 5: Make dummy WebAPP
1. TODO
##
Current notes
1. The client doens't preserve new lines, it should
2. Because The APIs are on different addresses, when trying to post we get an error due to CORS
3. The client should have a config for which api to call for pyhton.
4. the client need to respond on errors
5. the service should report back errors