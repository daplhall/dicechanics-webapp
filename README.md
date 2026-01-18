# (WIP) Dicechancis web app
![#f03c15](https://placehold.co/15x15/f03c15/f03c15.png) This project is currently WIP.
This is going to be a web app for [dicechanics](https://github.com/daplhall/dicechanics).  

The goal is to make a client in which one can dicechancis programs into, and print their probabilities as graphs.  

The dicechancis programs are gonna be a small interpreted language which is translated into python code. As it allows me to reuse my library.

A ASCII mock up can be seen here:
```
+----------------------------------+
| Program         | Visualiztion   |
+-----------------+----------------+
| d = d6          | #              |
| g = sum(2@d8)   | # #            |
| plot d+g        | # # #          |
|                 | 1 2 3          |
+----------------------------------+
```
## Goals for the project
1. Each service needs to be sepereate entities and should be a black box. 
2. The language should be easy to use, and mimick the Python syntax.
3. Have fun and experiment
## Booting up the services and client
### C# dependencies
1. [Python.net](https://pythonnet.github.io/), as it is used as the translation
unit between C# and Python.
### Bootup on Linux
Start by creating the docker daemon
```
systemctl start docker
```
then simply from project root invoke
```
docker compose up
```
The ports `8080` is used for the `pythonservice` and `4200` for the client.
# Attack Plan
Part 1: create a dummy API
- [x] Create an API that just take input and parses to output (Done)

Part 2: Invoking Pyhton
- [x] Create an object that invokes python
- [x] Make the python object parse python code
- [x] Get output from python execution

Part 3: Combine 1 & 2
- [x] Update api so one can send python code.
- [x] Update APi so pyhton output get send back as json

Part 4: Syntax Statemachine
- [ ] make a translation state machine 
- [ ] Create plot syntax
- [ ] Define new syntax

Part 5: Plotting in client
- TODO

Part 6: Additional features to make it a game design hub
- TODO

# Client test program
```
import json
print("hello world", flush=True)
output = {"a": 1}
output = json.dumps(output)
```
We json dumps because we hoot into the program to get output, and it needs to be 
    a json string for C# -> Python translation purposes. (look into [Codecs](https://pythonnet.github.io/pythonnet/codecs.html))