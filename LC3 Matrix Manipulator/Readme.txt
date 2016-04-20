This program written in LC3 Assembly is capable of modifying a matrix stored at 0x5004 with five different operations (Shift Left, Up, Down, Right and Transpose)
The number of rows is stored in 0x5000, columns in 0x5001, operation code in 0x5002 (1. Shift Left, 2. Right 3. Up 4. Down 5.Transpose)
If the operation specified is a shift operation, the amount to shift by is stored in 0x5003
The resulting matrix is stored in the same memory space beginning at 0x5004
