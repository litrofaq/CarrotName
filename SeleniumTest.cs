using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace CarrotName;

enum Gender { Boy, Girl }

public class PracticeParrotName
{
    private ChromeDriver driver;
    private string urlParrotName = "https://qa-course.kontur.host/selenium-practice/";
    private string titleFormText = "Мы подберём имя для твоего попугайчика!";
    private By titleFormLokator = By.ClassName("subtitle-bold");
    private string questionGenderFormText = "Кто у тебя?";
    private By questionGenderFormLokator = By.XPath("//*[@class='formQuestion'][1]");
    private string questionMailFormText = "На какой e-mail прислать варианты имён?";
    private By questionMailFormLokator = By.XPath("//*[@class='formQuestion'][2]");
    private string emailTrue = "test@mail.ru";
    private string email3LevelDomain = "test@mail.mail.ru";
    private string emailDomainIsDot = "test@.";
    private string emailLoginIsEmpty = "@mail.ru";
    private By radioBoyLokator = By.Id("boy");
    private By labelRadioBoyLokator = By.XPath("//*[@class='parrot']/label[1]");
    private string labelRadioBoyText = "мальчик";
    private By radioGirlLokator = By.Id("girl");
    private By labelRadioGirlLokator = By.XPath("//*[@class='parrot']/label[2]");
    private string labelRadioGirlText = "девочка";
    private By emailInputLokator = By.Name("email");
    private string placeholderText = "e-mail";
    private By buttonSendMeLokator = By.Id("sendMe");
    private string buttonSendMeText = "ПОДОБРАТЬ ИМЯ";
    private By resultTextLokator = By.ClassName("result-text");
    private By resultEmailLokator = By.ClassName("your-email");
    private By linkAnotherEmailLokator = By.Id("anotherEmail");
    private string linkAnotherEmailText = "указать другой e-mail";
    private By errorFormLokator = By.ClassName("form-error");


    private static string expectedResultGender(Gender gender)
    {
        string genderText = "Хорошо, мы пришлём имя для ";
        switch (gender)
        {
            case Gender.Boy:
                genderText += "вашего мальчика ";
                break;
            case Gender.Girl:
                genderText += "вашей девочки ";
                break;
        }

        genderText += "на e-mail:";
        return genderText;
    }

    [SetUp]
    public void SetUp()
    {
        // Опции отображения браузера
        var options = new ChromeOptions();
        options.AddArgument("start-maximized");
        // Запуск браузера
        driver = new ChromeDriver(options);
    }

    [Test]
    public void ParrotName_CheckElementsForm_Success()
    {
        Assert.Multiple(() =>
        {
            // Перейти по урлу
            driver.Navigate().GoToUrl(urlParrotName);

            // Ожидать 10 секунд загрузки формы
            WebDriverWait waitLoadForm = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            waitLoadForm.Until(e => e.FindElement(emailInputLokator)); //.SendKeys(emailTrue);    

            // Проверка наполнения формы -- start --------------------------------------------
            // Заголовок
            Assert.IsTrue(driver.FindElement(titleFormLokator).Displayed,
                "Заголовок формы не отображается");
            Assert.AreEqual(titleFormText, driver.FindElement(titleFormLokator).Text,
                $"\tЗаголовок формы в прототипе: '{titleFormText}'\n" +
                $"Фактический заголовок в форме: '{driver.FindElement(titleFormLokator).Text}'");
            // Вопрос формы для выбора пола
            Assert.IsTrue(driver.FindElement(questionGenderFormLokator).Displayed,
                "Вопрос формы не отображается");
            Assert.True(driver.FindElement(questionGenderFormLokator).Text.Contains(questionGenderFormText),
                $"\tВопрос в прототипе: '{questionGenderFormText}'\n" +
                $"Фактический вопрос в форме: '{driver.FindElement(questionGenderFormLokator).Text}'");
            // Список чекбоксов присутствует
            Assert.IsTrue(driver.FindElement(radioBoyLokator).Displayed,
                "Радио-баттон для мальчика отсутствует");
            Assert.True(driver.FindElement(labelRadioBoyLokator).Text.Contains(labelRadioBoyText),
                $"\tВариант в прототипе: '{labelRadioBoyText}'\n" +
                $"Фактический вариант в форме: '{driver.FindElement(labelRadioBoyLokator).Text}'");

            Assert.IsTrue(driver.FindElement(radioGirlLokator).Displayed,
                "Радио-баттон для девочки отсутствует");
            Assert.True(driver.FindElement(labelRadioGirlLokator).Text.Contains(labelRadioGirlText),
                $"\tВариант в прототипе: '{labelRadioGirlLokator}'\n" +
                $"Фактический вариант в форме: '{driver.FindElement(labelRadioGirlLokator).Text}'");

            // Вопрос формы для ввода емэйла
            Assert.IsTrue(driver.FindElement(questionMailFormLokator).Displayed,
                "Вопрос запроса емэйл не отображается");
            Assert.True(driver.FindElement(questionMailFormLokator).Text.Contains(questionMailFormText),
                $"\tВопрос в прототипе: '{questionMailFormText}'\n" +
                $"Фактический вопрос в форме: '{driver.FindElement(questionMailFormLokator).Text}'");

            // Поле ввода пустое
            Assert.IsEmpty(driver.FindElement(emailInputLokator).Text,
                $"\tОжидалось пустое поле ввода e-mail\n" +
                $"Фактическое значение поля ввода e-mail: '{driver.FindElement(buttonSendMeLokator).Text}'");

            //var dd = driver.FindElement(By.XPath("//*[@placeholder]")).GetTe//ComputedAccessibleLabel;
            //var ddd = driver.FindElement(emailInputLokator).GetAttribute("placeholder");
            Assert.True(driver.FindElement(emailInputLokator).GetAttribute("placeholder").Contains(placeholderText),
                $"\tТекст подсказки в прототипе: '{buttonSendMeText}'\n" +
                $"Фактический текст подсказки в поле ввода: '{driver.FindElement(emailInputLokator).GetAttribute("placeholder")}'");
            // Кнопка присутствует
            Assert.IsTrue(driver.FindElement(buttonSendMeLokator).Displayed,
                "Кнопка 'ПОДОБРАТЬ ИМЯ' отсутствует");
            Assert.True(driver.FindElement(buttonSendMeLokator).Text.Contains(buttonSendMeText),
                $"\tТекст кнопки в прототипе: '{buttonSendMeText}'\n" +
                $"Фактический текст кнопки в форме: '{driver.FindElement(buttonSendMeLokator).Text}'");

            // Проверка наполнения формы -- end ---------------------------------------------        
        });
    }

    [Test]
    public void ParrotName_SelectBoy_SendTrueEmailOnClickButton_Success()
    {
        // Перейти по урлу
        driver.Navigate().GoToUrl(urlParrotName);

        // Ожидать 10 секунд появления поля ввода
        WebDriverWait waitVisibleInput = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        waitVisibleInput.Until(e => e.FindElement(emailInputLokator)).SendKeys(emailTrue);



        // Выбрать мальчика
        driver.FindElement(radioBoyLokator).Click();


        // Кликнуть по кнопке
        driver.FindElement(buttonSendMeLokator).Click();
        Assert.Multiple(() =>
        {
            // Появился текст об отправке имени
            Assert.IsTrue(driver.FindElement(resultTextLokator).Displayed,
                "Сообщение о отправке имени не отображается");
            // Будет отправлено имя мальчика
            Assert.AreEqual(expectedResultGender(Gender.Boy), driver.FindElement(resultTextLokator).Text,
                $"\tПол питомца не соответствует выбранному\n" +
                $"Ожидается сообщение: '{expectedResultGender(Gender.Boy)}'\n" +
                $"Фактическое сообщение в форме: '{driver.FindElement(resultTextLokator).Text}'");
            // Емэйл отображается
            Assert.IsTrue(driver.FindElement(resultEmailLokator).Displayed,
                "Емэйл не отображается");
            // Емэйл введеный в поле
            Assert.AreEqual(emailTrue, driver.FindElement(resultEmailLokator).Text,
                $"Емэйл для отправки не соответствует введенному\n" +
                $"Емэйл, введенный в поле: '{emailTrue}'\n" +
                $"Фактически отображаемый емэйл для отправки: '{driver.FindElement(resultEmailLokator).Text}'");
            // Кнопка исчезла
            Assert.IsFalse(driver.FindElement(buttonSendMeLokator).Displayed,
                "Кнопка не исчезла");

            // Ссылка на другой емэйл есть
            Assert.IsTrue(driver.FindElement(linkAnotherEmailLokator).Displayed,
                "Ссылка 'указать другой e-mail' не отображается");
            Assert.True(driver.FindElement(linkAnotherEmailLokator).Text.Contains(linkAnotherEmailText),
                $"\tТекст ссылки не соответствует прототипу\n" +
                $"Текст ссылки в прототипе: '{linkAnotherEmailText}'\n" +
                $"Фактический текст ссылки в форме: '{driver.FindElement(linkAnotherEmailLokator).Text}'");
        });
    }


    [Test]
    public void ParrotName_SelectGirl_SendTrueEmailOnClickButton_Success()
    {
        // Перейти по урлу
        driver.Navigate().GoToUrl(urlParrotName);

        // Ожидать 10 секунд появления поля ввода
        WebDriverWait waitVisibleInput = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        waitVisibleInput.Until(e => e.FindElement(emailInputLokator)).SendKeys(emailTrue);



        // Выбрать девочку
        driver.FindElement(radioGirlLokator).Click();


        // Кликнуть по кнопке
        driver.FindElement(buttonSendMeLokator).Click();
        Assert.Multiple(() =>
        {
            // Появился текст об отправке имени
            Assert.IsTrue(driver.FindElement(resultTextLokator).Displayed,
                "Сообщение о отправке имени не отображается");
            // Будет отправлено имя девочки
            Assert.AreEqual(expectedResultGender(Gender.Girl), driver.FindElement(resultTextLokator).Text,
                $"\tПол питомца не соответствует выбранному\n" +
                $"Ожидается сообщение: '{expectedResultGender(Gender.Girl)}'\n" +
                $"Фактическое сообщение в форме: '{driver.FindElement(resultTextLokator).Text}'");
            // Емэйл отображается
            Assert.IsTrue(driver.FindElement(resultEmailLokator).Displayed,
                "Емэйл не отображается");
            // Емэйл введеный в поле
            Assert.AreEqual(emailTrue, driver.FindElement(resultEmailLokator).Text,
                $"Емэйл для отправки не соответствует введенному\n" +
                $"Емэйл, введенный в поле: '{emailTrue}'\n" +
                $"Фактически отображаемый емэйл для отправки: '{driver.FindElement(resultEmailLokator).Text}'");
            // Кнопка исчезла
            Assert.IsFalse(driver.FindElement(buttonSendMeLokator).Displayed,
                "Кнопка не исчезла");

            // Ссылка на другой емэйл есть
            Assert.IsTrue(driver.FindElement(linkAnotherEmailLokator).Displayed,
                "Ссылка 'указать другой e-mail' не отображается");
            Assert.True(driver.FindElement(linkAnotherEmailLokator).Text.Contains(linkAnotherEmailText),
                $"\tТекст ссылки не соответствует прототипу\n" +
                $"Текст ссылки в прототипе: '{linkAnotherEmailText}'\n" +
                $"Фактический текст ссылки в форме: '{driver.FindElement(linkAnotherEmailLokator).Text}'");
        });
    }

    [Test]
    public void ParrotName_SelectGirl_SendTrueEmailOnClickButton_ClickLinkAnotherEmail_Success()
    {
        // Перейти по урлу
        driver.Navigate().GoToUrl(urlParrotName);

        // Ожидать 10 секунд появления поля ввода
        WebDriverWait waitVisibleInput = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        waitVisibleInput.Until(e => e.FindElement(emailInputLokator)).SendKeys(emailTrue);



        // Выбрать девочку
        driver.FindElement(radioGirlLokator).Click();


        // Кликнуть по кнопке
        driver.FindElement(buttonSendMeLokator).Click();
        // Кликнуть по ссылке
        driver.FindElement(linkAnotherEmailLokator).Click();

        Assert.Multiple(() =>
        {
            // Ссылка исчезла
            Assert.IsFalse(driver.FindElement(linkAnotherEmailLokator).Displayed,
                "Ожидалось, что ссылка исчезнет");
            // Ответ исчез
            Assert.AreEqual(0, driver.FindElements(resultTextLokator).Count,
                "Ожидалось, что текст результата отправки исчезнет");
            // Емэйл из ответа исчез
            Assert.AreEqual(0, driver.FindElements(resultEmailLokator).Count,
                "Ожидалось, что емэйл результата исчезнет");
            // Поле ввода пустое
            Assert.IsEmpty(driver.FindElement(emailInputLokator).Text,
                $"\tОжидалось, что поле ввода e-mail очистится\n" +
                $"Фактическое значение поля ввода e-mail: '{driver.FindElement(buttonSendMeLokator).Text}'");

            Assert.True(driver.FindElement(emailInputLokator).GetAttribute("placeholder").Contains(placeholderText),
                $"\tОжидался текст подсказки в поле ввода: '{buttonSendMeText}'\n" +
                $"Фактический текст подсказки в поле ввода: '{driver.FindElement(emailInputLokator).GetAttribute("placeholder")}'");
            // Кнопка появилась
            Assert.IsTrue(driver.FindElement(buttonSendMeLokator).Displayed,
                "Кнопка 'ПОДОБРАТЬ ИМЯ' не появилась");
            // Надпись на кнопке соответствует
            Assert.True(driver.FindElement(buttonSendMeLokator).Text.Contains(buttonSendMeText),
                $"\tТекст кнопки в прототипе: '{buttonSendMeText}'\n" +
                $"Фактический текст кнопки в форме: '{driver.FindElement(buttonSendMeLokator).Text}'");
            // кнопка активна ? надо не?
            // Список чекбоксов присутствует
            Assert.IsTrue(driver.FindElement(radioBoyLokator).Displayed,
                "Радио-баттон для мальчика отсутствует");
            Assert.True(driver.FindElement(labelRadioBoyLokator).Text.Contains(labelRadioBoyText),
                $"\tВариант в прототипе: '{labelRadioBoyText}'\n" +
                $"Фактический вариант в форме: '{driver.FindElement(labelRadioBoyLokator).Text}'");

            Assert.IsTrue(driver.FindElement(radioGirlLokator).Displayed,
                "Радио-баттон для девочки отсутствует");
            Assert.True(driver.FindElement(labelRadioGirlLokator).Text.Contains(labelRadioGirlText),
                $"\tВариант в прототипе: '{labelRadioGirlLokator}'\n" +
                $"Фактический вариант в форме: '{driver.FindElement(labelRadioGirlLokator).Text}'");
            // Заголовок
            Assert.IsTrue(driver.FindElement(titleFormLokator).Displayed,
                "Заголовок формы не отображается");
            Assert.AreEqual(titleFormText, driver.FindElement(titleFormLokator).Text,
                $"\tЗаголовок формы в прототипе: '{titleFormText}'\n" +
                $"Фактический заголовок в форме: '{driver.FindElement(titleFormLokator).Text}'");
            // Вопрос формы для выбора пола
            Assert.IsTrue(driver.FindElement(questionGenderFormLokator).Displayed,
                "Вопрос формы не отображается");
            Assert.True(driver.FindElement(questionGenderFormLokator).Text.Contains(questionGenderFormText),
                $"\tВопрос в прототипе: '{questionGenderFormText}'\n" +
                $"Фактический вопрос в форме: '{driver.FindElement(questionGenderFormLokator).Text}'");
            // Вопрос формы для ввода емэйла
            Assert.IsTrue(driver.FindElement(questionMailFormLokator).Displayed,
                "Вопрос запроса емэйл не отображается");
            Assert.True(driver.FindElement(questionMailFormLokator).Text.Contains(questionMailFormText),
                $"\tВопрос в прототипе: '{questionMailFormText}'\n" +
                $"Фактический вопрос в форме: '{driver.FindElement(questionMailFormLokator).Text}'");

        });
    }

    [Test]
    public void ParrotName_SendEmail3LevelDomain_Success()
    {
        // Перейти по урлу
        driver.Navigate().GoToUrl(urlParrotName);

        // Ожидать 10 секунд появления поля ввода
        WebDriverWait waitVisibleInput = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        waitVisibleInput.Until(e => e.FindElement(emailInputLokator)).SendKeys(email3LevelDomain);
        // Кликнуть по кнопке
        driver.FindElement(buttonSendMeLokator).Click();


        Assert.Multiple(() =>
        {
            Assert.IsFalse(driver.FindElement(errorFormLokator).Displayed,
                $"\tОжидалась успешная валидация емэйла домена третьего уровня '{email3LevelDomain}'\n" +
                $"Фактически ошибка валидации емэйл: '{driver.FindElement(errorFormLokator).Text}'");
        });
    }

    [Test]
    public void ParrotName_SendEmaiLoginlIsEmpty_Failure()
    {
        // Перейти по урлу
        driver.Navigate().GoToUrl(urlParrotName);

        // Ожидать 10 секунд появления поля ввода
        WebDriverWait waitVisibleInput = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        waitVisibleInput.Until(e => e.FindElement(emailInputLokator)).SendKeys(emailLoginIsEmpty);
        // Кликнуть по кнопке
        driver.FindElement(buttonSendMeLokator).Click();


        Assert.Multiple(() =>
        {
            Assert.IsTrue(driver.FindElement(errorFormLokator).Displayed,
                $"\tОжидалась ошибка валидации емэйла без логина '{emailLoginIsEmpty}'\n");
        });
    }
    
    [Test]
    public void ParrotName_SendEmaiDomainlIsDot_Failure()
    {
        // Перейти по урлу
        driver.Navigate().GoToUrl(urlParrotName);

        // Ожидать 10 секунд появления поля ввода
        WebDriverWait waitVisibleInput = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        waitVisibleInput.Until(e => e.FindElement(emailInputLokator)).SendKeys(emailDomainIsDot);
        // Кликнуть по кнопке
        driver.FindElement(buttonSendMeLokator).Click();


        Assert.Multiple(() =>
        {
            Assert.IsTrue(driver.FindElement(errorFormLokator).Displayed,
                $"\tОжидалась ошибка валидации емэйла без домена '{emailDomainIsDot}'\n");
        });
    }    
    
    [TearDown]
    public void TearDown()
    {
        driver.Quit();
    }
}