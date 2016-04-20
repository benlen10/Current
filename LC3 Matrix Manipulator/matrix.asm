;Free to modify x4000 - x5000
.ORIG x3000
;Save Initial register values
STR R0, Start, #0
LD R0, Start
ADD R0, R0, #1
STR R1, R0, #0
ADD R0, R0, #1
STR R2, R0, #0
ADD R0, R0, #1
STR R3, R0, #0
ADD R0, R0, #1
STR R4, R0, #0
ADD R0, R0, #1
STR R5, R0, #0
ADD R0, R0, #1
STR R6, R0, #0
ADD R0, R0, #1
STR R7, R0, #0

;Load Operation value to R0 and determine label to jump to
LDI R0, OP
ADD R1, R0, #-6
BRpz INVALIDOP   ;Check for invalid op greater than 5
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







INVALIDOP
;Throw Error

DONE TRAP x25  
;Labels
Start  .FILL x4000
Length .FILL x5000
Length .FILL x5001
OP     .FILL x5002
Shift  .FILL x5003
Matrix .FILL x5004

	.END