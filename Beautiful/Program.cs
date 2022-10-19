/* Тестовое задание.
В данной задаче будут рассматриваться 13-ти значные числа в тринадцатиричной системе исчисления(цифры 0,1,2,3,4,5,6,7,8,9,A,B,C) с ведущими нулями.
Например: ABA98859978C0, 6789110551234, 0000007000000

A BA9 885 997 8C0

Назовем число красивым, если сумма его первых шести цифр равна сумме шести последних цифр.

Пример:
Число 0055237050A00 - красивое, так как 0+0+5+5+2+3 = 0+5+0+A+0+0
Число 1234AB988BABA - некрасивое, так как 1+2+3+4+A+B != 8+8+B+A+B+A​

Задача:
написать программу на С# печатающую в стандартный вывод количество 13-ти значных красивых чисел с ведущими нулями в тринадцатиричной системе исчисления.

В качестве решения должно быть предоставлено:
1) ответ - количество таких чисел. Ответ должен быть представлен в десятичной системе исчисления.
2) исходный код программы.
*/


using System.Diagnostics;

const string alphabet = "0123456789ABC";

var nn = 6;
var n = 13 * 13 * 13 * 13 * 13 * 13;

string GetValue13(int k)
{
    IEnumerable<char> Iterate(int k)
    {
        do
        {
            yield return alphabet[k % 13];

            k /= 13;
        } while (k > 0);
    }

    return new string(Iterate(k).Reverse().ToArray());
}

string GetPadNValue13(int k, int padCount) => GetValue13(k).PadLeft(padCount, '0');
string GetPadValue13(int k) => GetValue13(k).PadLeft(nn, '0');

int[][] GetTerms(int k, int length, int maxValue)
{
    var path = new List<int>(length);
    var sum = 0;

    IEnumerable<int[]> GetPathes()
    {
        if ((length - path.Count) * maxValue < k - sum)
            yield break;

        for (var i = 0; i < maxValue; i++)
        {
            sum += i;
            path.Add(i);

            if (sum == k && path.Count == length)
                yield return path.ToArray();

            if (sum <= k && path.Count < length)
                foreach (var goodPath in GetPathes())
                    yield return goodPath;

            sum -= i;
            path.RemoveAt(path.Count - 1);
        }
    }

    return GetPathes().ToArray();
}

int GetTermCount(int k, int termCount, int digitCount)
{
    var count = 0;
    var sum = 0;

    IEnumerable<int> GetPathes()
    {
        if ((termCount - count) * digitCount < k - sum)
            yield break;

        for (var i = 0; i < digitCount; i++)
        {
            sum += i;
            count++;

            if (sum == k && count == termCount)
                yield return count;

            if (sum <= k && count < termCount)
                foreach (var goodPath in GetPathes())
                    yield return goodPath;

            sum -= i;
            count--;
        }
    }

    return GetPathes().Count();
}

//var t = GetTerms(2, 6, 13);
//var c = GetTermCount(2, 6, 13);

// 7-е число. Первый мультипликатор
var m1 = 13;

// какое число красивых чисел соответствует одному шестизначному числу
//int CountBeauty(string k)
//{
//    var chars = k.ToArray();

//    var counter = 0;
//    for (var i = 0; i < nn; i++)
//    for (var j = i + 1; j < nn; j++)
//    {
//        if (chars[i] != chars[j])
//        {
//            counter++;
//        }
//    }

//    return counter;
//}

// какое число красивых чисел соответствует всем шестизначным числам 
var m2 = Enumerable.Range(0, n).AsParallel().Select(k => GetTermCount(k, nn, 13)).Sum();

var res = m1 * m2;

Debug.WriteLine($"Ответ: {res}");

Console.WriteLine(GetPadNValue13(res, 13));



