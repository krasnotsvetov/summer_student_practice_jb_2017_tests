# Task_1
Extract header information, bsjb, rsds and mpdb information from PE file in two modes:
1. Extract without loading to memory and execute PE file
2. Extract with loading and executing this PE file.

# How to use:

Example:

1.
  ```
  PEExtractor MiscEmbedded.dll -rv
  ```

  The PE file will be loaded to memory and executed( the second argument '-v') and the information will be reporting (-r)
  
  WARNING:: if you compile x64 mode, you can execute only x64 library.
            if you compile with x86 mode, you can execute only x86 library.

2.
  ```
  PEExtractor MiscEmbedded.dll -r
  ```
 The information will be extracted without executing PE file. So if you compile this PEExtractor in x86 mode, you can use it to extract
 information from different platforms, such as x86, x64, arm e.t.c/
 
