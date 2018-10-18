 find . -name *.cs | xargs grep "public interface" | sed -e 's/\(\S\+\):\s\+\(.*\)/### \2\n\1/g'
