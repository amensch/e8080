# eCPU
This is an Intel 8080 emulator written in C#.  This project mostly exists as a learning tool for myself.

The intent is to be able to fully play Space Invaders. The binaries that were originally used in video arcades are readily
available on the internet.

15-Dec-2015
All 8080 opcodes have been implemented including DAA and the auxillary carry flag.  
There exists a full suite of unit tests for the op codes and other functionality.

To be done is to complete the code for handling interrupts of user input as well as the graphics.
There is much refactoring to be done.  The purpose here was to provide a working learning tool for myself
using C#.  The focus is more on design and the use of automated tests rather than raw speed. 
