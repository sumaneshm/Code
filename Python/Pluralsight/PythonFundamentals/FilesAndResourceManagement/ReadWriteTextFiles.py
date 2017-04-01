import sys

def write_file_test():
    # open() function gives a file handle for us
    # mode tells us how to open the file.
    #   w = write mode, truncate the file first
    #   r = read mode (default)
    #   a = append mode, won't clear the file contents first
    #   x = exclusive write mode, if the file already exists, will fail
    #   b = binary content
    #   t = text content

    f = open("Wasteland.txt", mode='wt', encoding='utf-8')
    # write methods returns an integer to indicate how many characters are written to the file
    # we have to provide new line character (\n) when we need, there is no write line method
    f.write("This is a test, ")
    f.write("And I am done with my second line\n")
    f.write("Finally, we have come to an end of the file...")
    # until we close the file, the contents are not written to the file, so we have to remember to close it all the time
    f.close()


def read_file_test():
    g = open("Wasteland.txt", mode="rt", encoding="utf-8")

    # There are many ways to read the contents of a file

    print("\nMethod 1 : File as iterator")
    # Method 1 - When we have to iterate through the entire file, easiest way would be this
    for line in g:
        sys.stdout.write(line)

    # as we browsed through the contents, the file pointer would have come to the end
    # we need to move it back to the beginning of the file and for that, we have to "seek"    g.seek(0)
    g.seek(0)


    print("\nMethod 2 : read(n) characters")
    # Method 2 - Passing an integer to read would read only that many number of chars
    print("First ten chars ", g.read(10))

    print("\nMethod 3 : read full content as a string")
    # Method 3 - Calling a read without passing any parameter would read the entire
    # content from the current location into a string
    print("Remaining file content : ", g.read())

    print("\nMethod 4 : read line by line")
    # Method 4 - Reading the entire content by one line at a time
    g.seek(0)
    print("First line : ", g.readline())
    print("Second line : ", g.readline())
    print("Third line : ", g.readline())

    print("Method 5 : Read all the lines into a list, useful when we move back and front")
    g.seek(0)
    lines = g.readlines()
    print(lines)

    g.close()

if __name__ == "__main__":
    write_file_test()
    read_file_test()
