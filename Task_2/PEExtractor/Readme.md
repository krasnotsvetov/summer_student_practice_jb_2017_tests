# Task_2
Extract #BSJB information and all tables.

# How to use:

Example:

1.
  ```
  PEExtractor MiscEmbedded.bsjb
  ```
  Where MiscEbedded.bsjb should start from BSJB signature
2.
  ```
  PEExtractor MiscEmbedded.dll -f
  ```
  Extract from full PE FIle(-f argument)
  
  
  
 # Note:
 
 Report will have several files:
 <file_name>.Tables.Report - file which contains table information in readable form
 
 <file_name>.Strings.Report - #Strings heap report
 
 <file_name>.GUID.Report - #GUID heap report
 
 <file_name>.BSJBMetadata.Report - BSJB root structure report
 
 <file_name>.\~Stream.Report - #\~ stream root structure report
 
 <file_name>.PDBStream.Report - #Pdb stream root structure report
 
 
 # Note:
 File can be missed if stream or structure is not presented.
 
 # Warning:
 log4net.bsjb  - does not contains #~ stream, but contains #- stream.
 
 GameOfLife.bsjb - will crash program, because BSJB is not correct ( IMHO )
 
 GameOfLife.exe -f - will crash program too. (I throw exceptions, if can't decode data correct, here it is a CodedIndex in 0x0A Table)
