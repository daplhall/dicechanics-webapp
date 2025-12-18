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