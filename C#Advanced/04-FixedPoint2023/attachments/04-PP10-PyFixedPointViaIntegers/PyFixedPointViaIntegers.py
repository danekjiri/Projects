from numpy import *

x1 = uint8(0b00001_010)  # 1.25 in fixed-point 5.3

x2 = uint8(0b00001_111)  # 1.875 in fixed-point 5.3

x3 = uint8(0b00011_000)  # 3

x4 = uint8(0b00011_100)  # 3.5

print(x1)
print(x2)
print(x3)
print(x4)

print()

def PrintFixed5_3(value : uint8):
    # value is in fixed-point 5.3
    # value represents real number A: value = A * N, where N = 2 ** 3, i.e. N = 8
    print(value / (1 << 3)) # = value / (2 ** 3) = value / 8 

PrintFixed5_3(x1)
PrintFixed5_3(x2)
PrintFixed5_3(x3)
PrintFixed5_3(x4)

print()

def FloatToFixed5_3(value : float) -> uint8:
    return uint8(value * (1 << 3))

print(FloatToFixed5_3(1.25))
print(FloatToFixed5_3(1.875))
print(FloatToFixed5_3(3))
print(FloatToFixed5_3(3.5))

print()

x5 = FloatToFixed5_3(1.2)
print(x5)
PrintFixed5_3(x5)

x6 = FloatToFixed5_3(3.14)
print(x6)
PrintFixed5_3(x6)

# 5-bits -> available range of intergers: 0 .. (2 ** 5)-1 -> 0 .. 31
x7 = FloatToFixed5_3(33.14)
print(x7)
PrintFixed5_3(x7)

print()
print("Press ENTER to continue ...")
input()

# fixed-point 2.6 -> 8 bits total 
def PrintFixed2_6(value : uint8):
    print(value / (1 << 6))

def FloatToFixed2_6(value : float) -> uint8:
    return uint8(value * (1 << 6))

# fixed-point 2.14 -> 16 bits total
def PrintFixed2_14(value : uint16):
    print(value / (1 << 14))

def FloatToFixed2_14(value : float) -> uint16:
    return uint16(value * (1 << 14))

# fixed-point 2.30 -> 32 bits total
def PrintFixed2_30(value : uint32):
    print(value / (1 << 30))

def FloatToFixed2_30(value : float) -> uint32:
    return uint32(value * (1 << 30))

a5_3 = FloatToFixed5_3(0.25)
PrintFixed5_3(a5_3)

a2_6 = FloatToFixed2_6(0.25)
PrintFixed2_6(a2_6)

a2_14 = FloatToFixed2_14(0.25)
PrintFixed2_14(a2_14)

a2_30 = FloatToFixed2_30(0.25)
PrintFixed2_30(a2_30)

print()

a5_3 = FloatToFixed5_3(0.1)
PrintFixed5_3(a5_3)

a2_6 = FloatToFixed2_6(0.1)
PrintFixed2_6(a2_6)

a2_14 = FloatToFixed2_14(0.1)
PrintFixed2_14(a2_14)

a2_30 = FloatToFixed2_30(0.1)
PrintFixed2_30(a2_30)