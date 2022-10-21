/* Тестовое задание.
В данной задаче будут рассматриваться 13-ти значные числа в тринадцатиричной системе исчисления(цифры 0,1,2,3,4,5,6,7,8,9,A,B,C) с ведущими нулями.
Например: ABA98859978C0, 6789110551234, 0000007000000

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


using System.Collections.Concurrent;
using System.Diagnostics;
using Beautiful;

//ForEacher1.ForEach1();
//return;

const string alphabet = "0123456789ABC";

var sys = 13; 
var nn = 6;

var n = Enumerable.Range(0, nn).Aggregate(1, (b, _) => sys * b);

string GetValue13(long k)
{
    IEnumerable<char> Iterate(long k)
    {
        do
        {
            yield return alphabet[(int)(k % sys)];

            k /= sys;
        } while (k > 0);
    }

    return new string(Iterate(k).Reverse().ToArray());
}

int GetDigitSum13(int k)
{
    IEnumerable<int> Iterate(int k)
    {
        do
        {
            yield return k % sys;

            k /= sys;
        } while (k > 0);
    }

    return Iterate(k).Sum();
}

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

    IEnumerable<bool> GetCounts()
    {
        if ((termCount - count) * digitCount < k - sum)
            yield break;

        for (var i = 0; i < digitCount; i++)
        {
            sum += i;
            count++;

            if (sum == k && count == termCount)
                yield return true;

            if (sum <= k && count < termCount)
                foreach (var goodPath in GetCounts())
                    yield return goodPath;

            sum -= i;
            count--;
        }
    }

    return GetCounts().Count();
}


//var t = GetTerms(2, 6, 13);
//var c = GetTermCount(2, 6, 13);

// 7-е число. Первый мультипликатор
var m1 = sys;

var termCountCache = new ConcurrentDictionary<int, long>();

// какое число красивых чисел соответствует всем шестизначным числам 
var m2 = Enumerable.Range(0, n)
    .Select(GetDigitSum13)
    .AsParallel()
    .Select(digitSum => termCountCache.GetOrAdd(digitSum, termCount => GetTermCount(termCount, nn, sys)))
    .Sum();

var res = m1 * m2;

Debug.WriteLine($"Ответ: {GetValue13(res)} ({res})");
Console.WriteLine(GetValue13(res));



