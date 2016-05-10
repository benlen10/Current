;Free to modify x4000 - x5000
.ORIG x3000
;Save Initial register values
;LD R0, Start
;STR R0, R0, #0
;ADD R0, R0, #1
;STR R1, R0, #0
;ADD R0, R0, #1
;STR R2, R0, #0
;ADD R0, R0, #1
;STR R3, R0, #0
;ADD R0, R0, #1
;STR R4, R0, #0
;ADD R0, R0, #1
;STR R5, R0, #0
;ADD R0, R0, #1
;STR R6, R0, #0
;ADD R0, R0, #1
;STR R7, R0, #0

;Load Operation value to R0 and determine label to jump to
LDI R0, OP
ADD R1, R0, #-6
BRzp INVALIDOP   ;Check for invalid op greater than 5
ADD R1, R0, #-1
BRz LSHIFT
ADD R1, R0, #-2
BRz RSHIFT
ADD R1, R0, #-3
BRz USHIFT
ADD R1, R0, #-4
BRz DSHIFT
ADD R1, R0, #-5
BRz TRANSPOSE
BR DONE




RSHIFT 
LDI R1, Shift
LDI R2, Cols
LDI R3, Rows
ADD R0, R1, R2 ;Check if amount to shift = width
BRz DONE

LD R0, Matrix  ;Initialize Current position at end of beginning of matrix
BR SKIPOUTLOOP1

OUTLOOP1
LDI R0, Pos   ;Reload position for next col
LDI R1, Shift ;Reload shift counter for next col
ADD R0, R0, #1 

SKIPOUTLOOP1
LDI R2, Cols  ;Reload initial col value to R2
ADD R0, R0, R2
ADD R0, R0, #-1
STI R0, Pos
LOOP1
LDI R0, Pos
LDI R2, Cols  ;Reload initial col value to R2
LDR R6, R0, 0  ;Load decimal value to temp register
STI R6 EndValue  ;Load last value of the row to EndValue
ADD R0, R0, -1 ;Decrement once outside of loop
ADD R2, R2, #-1
INLOOP1 
LDR R6, R0, 0   ;Load decimal value to temp register
STR R6, R0, #1   ;Save pos to (pos+1)
ADD R0, R0, #-1 ;Shift Pointer Left
Add R2, R2, #-1 ;Decrement col count
BRp INLOOP1     ;Continue inter loop until pos pointer hits beginning of row -1
LDI R5, EndValue ;Load end value to a temp register, R5
STR R5, R0,1     ;Replace the first value of the row with EndValue
ADD R1, R1, #-1   ;Decerement Shift counter
BRp LOOP1         ;If shift counter is above zero, loop back to outer loop1
ADD R3, R3, #-1  ;Decrement row counter
BRp OUTLOOP1  ;If row counter is above zero, loop back to outer loop1
BR DONE 




LSHIFT
LDI R1, Shift
LDI R2, Cols
LDI R3, Rows
ADD R0, R1, R2 ;Check if amount to shift = width
BRz DONE

LD R0, Matrix  ;Initialize Current position at end of beginning of matrix
BR SKIPOUTLOOP2

OUTLOOP2
;LDI R0, Pos   ;Reload position for next col
LDI R1, Shift ;Reload shift counter for next col
;ADD R0, R0, R2
;ADD R0, R0, #-1




BR SKIPOUTLOOP2
LOOP2
LDI R0, Pos
SKIPOUTLOOP2
STI R0, Pos
LDI R2, Cols  ;Reload initial col value to R2
LDR R6, R0, 0  ;Load decimal value to temp register
STI R6 EndValue  ;Load last value of the row to EndValue
ADD R0, R0, #1 ;Increment once outside of loop
ADD R2, R2, #-1
INLOOP2 
LDR R6, R0, 0   ;Load decimal value to temp register
STR R6, R0, #-1   ;Save pos to (pos-1)
ADD R0, R0, #1 ;Shift Pointer Right
Add R2, R2, #-1 ;Decrement col count
BRp INLOOP2     ;Continue inter loop until pos pointer hits beginning of row -1
LDI R5, EndValue ;Load end value to a temp register, R5
STR R5, R0,#-1     ;Replace the last value of the row with EndValue
ADD R1, R1, #-1   ;Decerement Shift counter
BRp LOOP2         ;If shift counter is above zero, loop back to outer loop1
ADD R3, R3, #-1  ;Decrement row counter
BRp OUTLOOP2  ;If row counter is above zero, loop back to outer loop1
BR DONE 


DSHIFT   ;Invert downshift to upshift
LDI R1, Shift
LDI R2, Rows
SUBT
ADD R2, R2, #-1
ADD R1, R1, #-1
BRp SUBT
STI R2, Shift
BR USHIFT


USHIFT 
LDI R1, Shift
LDI R3, Cols
LDI R2, Rows
AND R0, R0, #0
MULTI2 ADD R0, R0, R3  ;R0 is the current element pointer
ADD R2, R2, #-1  
BRp MULTI2
ADD R2, R0, #0 ;R2 is the total elemenet counter
AND R0, R0, #0
STI R2, Elements
LD R4, Matrix
ADD R0, R0, R4
;ADD R0, R0, #-1
STI R0, Pos

OUTLOOP4
LDI R3, Cols

LOOP4   ;Keeps track of single shifts (not total row shifts)
LDI R2, Elements
ADD R2, R2, #-1
LDI R0, Pos   ;Begin ponter at beginning of matrix
LDI R6, Matrix  ;Load decimal value to temp register
STI R6, EndValue  ;Load first value of the matrix to EndValue
ADD R0, R0, #1   ;Increment once outside of loop
INLOOP4 
LDR R6, R0, 0   ;Load decimal value to temp register
STR R6, R0, #-1   ;Save pos to (pos-1)
ADD R0, R0, #1 ;Shift Pointer Right
ADD R2, R2, #-1 ;
BRp INLOOP4     ;Continue inter loop until pos pointer hits beginning of row -1
LDI R5, EndValue ;Load end value to a temp register, R5
STR R5, R0,#-1     ;Replace the last value of the matrix with EndValue
Add R3, R3, #-1 ;Decrement col count
BRp LOOP4         ;If shift counter is above zero, loop back to outer loop1
ADD R1, R1, #-1   ;Decerement Shift counter
BRp OUTLOOP4
BR DONE 




INVALIDOP AND, R0, R0, #0
TRANSPOSE AND, R0, R0, #0
;Throw Error



DONE TRAP x25  
;Labels
Rows   .FILL x5000
Cols   .FILL x5001
OP     .FILL x5002
Shift  .FILL x5003
Matrix .FILL x5004
Start  .FILL x4000 ;Store registers in x4000 - x4007
Tmp1   .FILL x4010 
EndValue .FILL x4011
Pos      .FILL x4012
Elements .FILL x4013


	.END