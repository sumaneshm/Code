import unittest
import os


def analyze_text(filename):
    lines = 0
    chars = 0

    with open(filename, mode="rt", encoding="utf-8") as f:
        for line in f:
            lines += 1
            chars += len(line)

    return (lines, chars)

# test cases will be inherited from unittest.TestCase
class TextAnalysisTests(unittest.TestCase):

    # Test initializers - will be called before "EACH" test
    def setUp(self):
        self.filename = "text_analysis_test_file.txt"
        with open(self.filename, mode="wt",encoding="utf-8") as f:
            f.write("This is line 1 \n"
                    "And this is the second line \n"
                    "And line three \n"
                    "THE LAST LINE"
                    )

    # test tear downs called after every test case
    def tearDown(self):
        try:
            os.remove(self.filename)
        except:
            pass

    # test method will have to start with "test_"
    def test_function_runs(self):
        analyze_text(self.filename)

    def test_line_count(self):
        self.assertEqual(analyze_text(self.filename)[0], 4)

    def test_char_count(self):
        self.assertEqual(analyze_text(self.filename), (4, 74))

    def test_no_such_file(self):
        # self.assertRaises checks whethe expected exception has been thrown
        with self.assertRaises(IOError):
            analyze_text('foobar')

    def test_no_deletion(self):
        analyze_text(self.filename)
        self.assertTrue(os.path.exists(self.filename))

if __name__ == '__main__':
    # unittest.main will automatically look for all the unittests and execute them
    unittest.main()
