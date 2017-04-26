# Отборочные задания по проекту "Восстановление параметров матрицы переходов условного вырадения в branch coverage" (Profiler С++ Team)
Задания можно писать на языках C# или C++. Использование С++ для решения дает дополнительные баллы, но требует от вас обязательного использования как минимум стандарта C++11 (lambdas, namespaces, SFINAE, smart pointers, ...). Использование сторонних библиотек в С++ разрешено (boost, gtest, ...), но весь парсинг данных вы должны написать сами.

Знание английского языка для чтения сопутствующей документации обязательно так как русского аналога просто не существует.

Решение нескольких задач увеличивает ваши шансы. Но, пожалуйста, расчитывайте свои силы и сделайте хотя бы одно задание!

**Важно:** Все ваши решения должны быть представлены как fork данного репозитория на GitHub.

## Задание №1
Написать программу для извлечения метадата секции (`BSJB` блок - `pecoff.docx` 3.4.3 offset 208/224 и 6.10, который недокументирован, но описан в `winnt.h` как `IMAGE_COR20_HEADER`) и информации неоходмой для отладки (`pecoff.docx` 3.4.3 offset 144/160; `RSDS` - `pecoff.docx` 6.1.1 `IMAGE_DEBUG_TYPE_CODEVIEW`, `MPDB` - `PE-COFF.md` type 17) из PE-файлов:
* расположенных прямо на диске;
* предварительно загруженных в память текущего процесса при помощи функции Win32 API `LoadLibraryEx`;

Парсить `BSJB` блок не нужно. `MPDB` желательно распаковать перед записью.

[Тестовые данные](/test_data/pe)

Базовае ссылки по теме:
  * https://msdn.microsoft.com/en-us/library/windows/desktop/ms680547.aspx (pecoff.docx)
  * https://github.com/dotnet/corefx/blob/master/src/System.Reflection.Metadata/specs/PE-COFF.md

## Задание №2
Написать программу для вывода в текстовый файл содержимого `BSJB` блока (`ECMA-335.pdf` II.24 + `PortablePdb-Metadata.md` как расширение ECMA-335) в читаемом формате.

Парсить и выводить данные из `#Blob` стрима метаданных не нужно.

[Тестовые данные](/test_data/md)

Базовае ссылки по теме:
  * https://www.ecma-international.org/publications/files/ECMA-ST/ECMA-335.pdf
  * https://github.com/dotnet/corefx/blob/master/src/System.Reflection.Metadata/specs/PortablePdb-Metadata.md
