
def write_grayscale(filename, pixels):
    # pixles = should be iterable list of iterable ints
    #           i.e. list[list[int]]
    #           int => Pixel
    #           inner list[int] = one row
    #           list[list[int]] = rows

    height = len(pixels)
    width = len(pixels[0])

    # with statement is much like using statement in C# such that it will automatically
    # call "close" when it goes out of scope and is a nice sugar coat.
    with open(filename, 'wb') as bmp:
        # bmp files header should be hardcoded BM
        bmp.write(b'BM')

        # get a bookmark to fill-up the size later
        size_bookmark = bmp.tell()
        bmp.write(b'\x00\x00\x00\x00')  # four bytes hold the filesize as a 32-bit

        bmp.write(b'\x00\x00')          # unused should be zero
        bmp.write(b'\x00\x00')

        pixel_offset_bookmark = bmp.tell()
        bmp.write(b'\x00\x00\x00\x00')  # integer offset to the pixel data


        # image header to be hard codedt to 40 decimals (40 = 0x28 in hexadecimal)
        bmp.write(b'\x28\x00\x00\x00')
        bmp.write(_int32_to_bytes(width))
        bmp.write(_int32_to_bytes(height))
        bmp.write(b'\x01\x00')
        bmp.write(b'\x08\x00')
        bmp.write(b'\x00\x00\x00\x00')
        bmp.write(b'\x00\x00\x00\x00')
        bmp.write(b'\x00\x00\x00\x00')
        bmp.write(b'\x00\x00\x00\x00')
        bmp.write(b'\x00\x00\x00\x00')
        bmp.write(b'\x00\x00\x00\x00')

        for c in range(256):
            bmp.write(bytes(c, c, c, 0))

        pixel_data_bookmark = bmp.tell()
        for row in reversed(pixels): #bmp files are bottom up, right to left
            row_data = bytes(row)
            bmp.write(row_data)
            padding = b'\x00' * (4 - len(row) % 4)
            bmp.write(padding)

        eof_bookmark = bmp.tell()

        bmp.seek(size_bookmark)
        bmp.write(_int32_to_bytes(eof_bookmark))

        bmp.seek(pixel_offset_bookmark)
        bmp.write(_int32_to_bytes(pixel_data_bookmark))


def _int32_to_bytes(i):
    return bytes((i & 0xff,
                  i >> 8 & 0xff,
                  i >> 16 & 0xff,
                  i >> 24 & 0xff))







