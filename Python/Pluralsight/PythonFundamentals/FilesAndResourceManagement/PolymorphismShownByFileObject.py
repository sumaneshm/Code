from urllib.request import urlopen

def words_per_line(f):
    return list(len(line.split()) for line in f)


def count_in_text_file():
    with open('Wasteland.txt', mode='rt', encoding='utf-8') as f:
        print(words_per_line(f))
        print(type(f))  # <class '_io.TextIOWrapper'>

def count_in_web():
    with urlopen('http://sixty-north.com/t.txt') as w:
        print(words_per_line(w))
        print(type(w))

if __name__ == '__main__':
    count_in_web()