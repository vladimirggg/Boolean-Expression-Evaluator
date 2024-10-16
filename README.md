# Интерпретатор на логически изрази

## Условие

Да се направи програма, позволяваща работа с логически изрази. Програмата трябва да има конзолен интерфейс, като потребителят трябва да може да изпълни някоя от следните команди: **DEFINE**, **SOLVE,** **ALL** и **FIND**.

### 1\. Дефиниране на логически функции

Командата **DEFINE** предоставя възможност на потребителя да дефинира логическа функция със зададено име. Логическите функции могат да са съставени от базовите логически операции **AND**, **OR** и **NOT** или други вече въведени от потребителя функции. Въвеждането на логическа функция трябва да проверява за наличие на грешки и може да бъде в [инфикс](https://en.wikipedia.org/wiki/Infix_notation) или [постфикс](https://en.wikipedia.org/wiki/Reverse_Polish_notation) (обратен полски запис), според предпочитанията на студента.

## Примери (в инфикс):

**DEFINE func1(a, b): "a & b"** //дефинира функция **func1** с операнди **a** и **b**, извършваща логическата операция **AND** между тях;

**DEFINE func2(a, b, c): "func1(a, b) | c"** //дефинира функция **func2**, извършваща логическата операция определена от **func1** между операндите **a** и **b**, и логическа операция **OR** между резултата от **func1** и операнда **c**. Функцията **func1** трябва да е била дефинирана от потребителя преди дефинирането на функция **func2**.

**DEFINE func3(a, b, c, d): "a & (b | c) & \!d"**

**DEFINE func4(a, b, c): "a & b | c | \!d"** //грешка операндът **d** не е дефиниран

**DEFINE func5(a, b, c, d): "func1(a, b) & func2(a, b, c) & func3(d)"** //грешка функцията **func3** се нуждае от четири параметъра, а е използвана само с един.

При изпълнение на команда **DEFINE**, студентът трябва да разчете въведения от потребителя текст и да обработи отделните му части:

* **DEFINE** \- наименование на командата;  
  * **funcX(a, b, c, d)** \- наименование на дефинираната функция и поредност на аргументите ѝ;  
  * **((a & b) | c) | \!d** \- логически израз (тяло на функцията).  
    

	Разчитането на командата трябва да се извърши символ по символ, като НЕ е разрешено ползването на готови функции като String.Split, String.IndexOf, функциите на  RegEx, LINQ и др. Ако студентът иска да ползва някоя от тях, трябва да разпише нейната функционалност сам.

	От прочетения израз студентът трябва да изгради дърво, в което всеки възел е операнд или оператор:

![][image1]  
Пример за дърво, отговарящо на израза: **((a & b) | c) | \!d**

    
Коренът на дървото, заедно с наименованието на функцията (в случая **funcX**), трябва да се запазят в хеш таблица, в която ключ е името на функцията. Всякакви структури различни от масив и динамичен масив (в това число хеш таблица, стек, опашка дърво и тн.) трябва да се реализират от студента.

### 2\. Решаване на функция за дадени параметри

Командата **SOLVE** дава възможност на потребителя да реши някоя от дефинираните функции за определени стойности на аргументите.

## Примери:

**SOLVE func1(1, 0\)** \-\> Result: 0;

**SOLVE func2(1, 0, 1\)** \-\> Result: 1;

При изпълнение на команда **SOLVE**, студентът трябва да разчете въведения от потребителя текст и да обработи отделните му части:

* **SOLVE** \- наименование на командата;  
  * **funcX(1, 0, 1, 0\)** \- наименование на дефинираната функция и стойности на аргументите;

Разчитането на командата трябва да се извърши символ по символ, като НЕ е разрешено ползването на готови функции като String.Split, String.IndexOf, Regex, LINQ и др. Ако студентът иска да ползва някоя от тях, трябва да разпише нейната функционалност сам.

От прочетения израз студентът трябва да намери запазената в хеш таблицата дефинирана функция, отговаряща на името **funcX**. От намерения запис се вижда, че поредността на аргументите е **(a, b, c, d)**, а от командата **SOLVE** се вижда, че стойностите за тях са (**1, 0, 1, 0**). 

Всеки от възлите **a, b, c** и **d** трябва да бъде намерен в дървото и за него трябва да се зададе съответната стойност. 

Трябва да се реализира рекурсивен метод, решаващ дървото, след изпълнението на който резултатът от изпълнението на целия израз трябва да се намира в корена на дървото.

При повторно изпълнение на команда **SOLVE** за функция, която вече е решавана, да се преизчисляват само частите от израза, за които това е необходимо. Например, ако се изчислява резултатът за функцията **func2(1, 0, 0\)** и след това за **func2(1, 0, 1\)**, това трябва да доведе до преизчисление само на операцията **OR**, тъй като **func1** вече е била изчислена за тези аргументи при първото решаване на **func2**.

### 3\. Изготвяне на таблица на истинност за логическа функция

Командата **ALL** дава възможност на потребителя да реши някоя от дефинираните функции за всички възможни стойности на аргументите.

    Пример:

   	 **ALL func1** \-\>	a , b : func1  
                         	0 , 0 : 0  
                         	0 , 1 : 0  
                         	1 , 0 : 0  
                         	1 , 1 : 1

Разчитането на командата трябва да се извърши символ по символ, като НЕ е разрешено ползването на готови функции като String.Split, String.IndexOf, Regex, LINQ и др. Ако студентът иска да ползва някоя от тях, трябва да разпише нейната функционалност сам.

От прочетения израз студентът трябва да намери запазената в хеш таблицата дефинирана функция, отговаряща на името **funcX**. От намерения запис се вижда, че поредността на аргументите е (**a, b, c, d)**. Трябва да се генерират всички възможни вариации на четирите аргумента и да се използва реализираният в т.3 метод за решаване на функция за всяка от вариациите.

### 5\. Намиране на логическа функция

Командата **FIND** дава възможност на потребителя да намери логическа функция, която не е налична (дефинирана), по въведена таблица на истинност. Логическата функция може да е съставена от всички базови или дефинирани от потребителя функции. Търсенето трябва да може да се извършва и паралелно, в множество нишки.

## Пример:  
      	    FIND  	0,0,0:0;  
                 	0,0,1:0;  
                 	0,1,0:0;  
                 	0,1,1:0;  
                 	1,0,0:0;  
                 	1,0,1:0;  
                 	1,1,0:0;  
                 	1,1,1:1

или чрез файл, в който последната колона е за резултата, а предходните са за операндите.

## Пример:   
**FIND** "d:\\table.csv"

Result: "a & b & c" или "func1(a, b) & c"

	За реализацията на това условие може да бъде предложен подход от студента. Някои от възможните варианти са:

1. Търсене чрез пълно изчерпване. Програмата трябва да генерира функции (дървета) с нужния брой аргументи (в примера 4). Ако допуснем, че потребителят е дефинирал само една функция (func1), тогава някои от възможните функции са:  
     
* a & b & c & d;  
* a & b & c | d;  
* …  
* func1(a, b) & c & d;  
* …  
* func1(func1(a, b), func1(c, d));  
* …

2. Търсене, чрез евристичен метод. Възможна е евристична реализация, например чрез генетичен алгоритъм. За целта трябва да се реализират операции по кръстосване на дървета, което е подход на [генетичното програмиране](https://en.wikipedia.org/wiki/Genetic_programming). 

Очевидно е, че няма горна граница за броя използвани функции, което налага търсенето да продължи до намиране на съвпадение или до изтичане на максимално допустимо време. Акцент в случая са бързината и ефективността на операциите, като може да се използват техники като branch & bound.

### 6\. Визуализация на функция\*

Визуализация на функция като дървовидна структура чрез графичен интерфейс (GDI), с възможност за показване на изчислената за всеки възел стойност.

## Пример:  
![][image1]  
Фиг. 1\. Визуализация на функцията като дървовидна структура.

## Реализация и точки

Всички функции за обработка на текст трябва да се реализират от студента (не е разрешено използването на функциите string.Split, string.IndexOf, Regex, функциите на LINQ и т.н.). Всички помощни структури и типове трябва да се реализират от студента, в това число стекове, свързани списъци, хеш таблици, дървета и т.н.

	Студентът трябва да реализира програмата в следната задължителна последователност:  
	

1. Въвеждане на логически функции.   
   	Допустимо е изразите да се въвеждат чрез обратен полски запис или както са показани в примерите.   
     
   Макс. брой точки за реализация: 15;  
     
2. Запис и четене от файл.  
   Функциите за запис и четене трябва да записват информацията в текстов (четим) вид.  
     
   Макс. брой точки за реализация: 5;  
     
3. Решаване на функция за дадени параметри.   
   Студентът трябва да реализира разнасяне на изчислените стойности от листата на дървото (операндите на израза), към корена (крайният резултат). Изчислението трябва да бъде рекурсивно.  
     
   Макс. брой точки за реализация: 15;

		

4. Изготвяне на таблица на истинност за логическа функция.   
   Не е разрешено използването на готови функции за генериране на вариации за операндите.  
     
   Макс. брой точки за реализация: 5;  
     
5. Намиране на логическа функция.  
   Ако търсенето се реализира чрез пълно изчерпване, трябва да се реализират всички възможни вариации на дърво с един възел, с два възела и тн, стига дърветата да са с коректен брой операнди.   
     
   Макс. брой точки за реализация: 20;  
     
6. \* \- Тази точка е незадължителна. При реализиране на всички точки, вкл. незадължителната, студентът ще бъде освободен от изпит с отлична оценка.


[image1]: <data:image/png;base64,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>