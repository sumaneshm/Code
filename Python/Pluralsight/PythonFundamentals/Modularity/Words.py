import sys
from urllib.request import urlopen


def get_words(url):
    with urlopen(url) as story:
        story_words = []
        for line in story:
            line_words = line.decode('utf-8').split()
            for word in line_words:
                story_words.append(word)
    return story_words


def print_words(words):
    for word in words:
        print(word)


def main(url):
    story_words = get_words(url)
    print_words(story_words)
















































if __name__ == "__main__":
    # 'https://www.sixty-north.com/c/t.txt'
    main(sys.argv[1])
