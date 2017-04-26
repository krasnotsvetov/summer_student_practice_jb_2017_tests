# Отборочные задания по проекту "Восстановление параметров матрицы переходов условного выражения в branch coverage" (Profiler С++ Team)
Задания можно писать на языках C# или C++, если иное не указано в самом задании. Использование С++ для решения дает дополнительные баллы, но требует от вас обязательного использования как минимум стандарта C++11 (lambdas, namespaces, SFINAE, smart pointers, ...). Использование сторонних утилитных библиотек в С++ таких как [boost](http://www.boost.org/), [gtest](https://github.com/google/googletest) разрешено, но всю логику работы вашей программы вы должны написать сами.

Знание английского языка для чтения сопутствующей документации обязательно, так как русского аналога просто не существует.

Решение нескольких задач увеличивает ваши шансы. Но, пожалуйста, расчитывайте свои силы и сделайте хотя бы одно задание хорошо!

**Важно:** Все ваши решения должны быть представлены как fork данного репозитория на GitHub.

## Задание №1
Написать программу для извлечения метадата секции (`BSJB` блок - [`pecoff.docx`](https://msdn.microsoft.com/en-us/library/windows/desktop/ms680547.aspx) 3.4.3 offset 208/224 и 6.10, который недокументирован, но описан в `winnt.h` как `IMAGE_COR20_HEADER`) и информации неоходмой для отладки ([`pecoff.docx`](https://msdn.microsoft.com/en-us/library/windows/desktop/ms680547.aspx) 3.4.3 offset 144/160; `RSDS` блок - [`pecoff.docx`](https://msdn.microsoft.com/en-us/library/windows/desktop/ms680547.aspx) 6.1.1 `IMAGE_DEBUG_TYPE_CODEVIEW`, `MPDB` блок - [`PE-COFF.md`](https://github.com/dotnet/corefx/blob/master/src/System.Reflection.Metadata/specs/PE-COFF.md) type 17) из PE-файлов:
* расположенных прямо на диске;
* предварительно загруженных в память текущего процесса при помощи функции Win32 API `LoadLibraryEx`;

Парсить `BSJB` блок не нужно. `MPDB` желательно распаковать перед записью. [Тестовые данные](/test_data/pe) для задания находятся в репозитории.

## Задание №2
Написать программу для вывода в текстовый файл содержимого `BSJB` блока ([`ECMA-335.pdf`](https://www.ecma-international.org/publications/files/ECMA-ST/ECMA-335.pdf) II.24 и [`PortablePdb-Metadata.md`](https://github.com/dotnet/corefx/blob/master/src/System.Reflection.Metadata/specs/PortablePdb-Metadata.md) целиком) в читаемом формате.

Парсить и выводить данные из `#Blob` стрима метаданных не нужно. [Тестовые данные](/test_data/md) для задания находятся в репозитории.

## Задание №3
Написать многопоточный [grep](https://en.wikipedia.org/wiki/Grep) по каталогу, с возможностью поиска с/без учета регистра.

Требования к реализации:
* Задание должно быть сделано на C++11/14/17
* Работа с FS происходит через [`boost::filesystem`](www.boost.org/doc/libs/release/libs/filesystem/) или [`std::experimental::filesystem`](http://en.cppreference.com/w/cpp/header/experimental/filesystem)
* Exception-safe code
* Кроссплатформенность
