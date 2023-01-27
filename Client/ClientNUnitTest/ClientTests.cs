namespace ClientNUnitTest
{
    public class Test
    {
        [SetUp]
        public void Setup()
        {
            //I setup kan vi istället instansiera vårat MathClass math variabel
        }


        //Demonstraion av ett test man kan köra
        [Test]
        public void TestExample()
        {
            // ARRANGE
            //var MathClass math = new MathClass(); 

            // ACT
            //int result = math.Add(1,1);

            // ASSERT
            //här använder vi assert för att se att resultatet är det vi förväntar oss att det ska vara, dvs att Add av 1+1 = 2
            // Assert.AreEqual(2, result);
            Assert.Fail();
        }

        [Test] 
        public void TestClientName() 
        {
            //ARRANGE
            Client client = new Client();

            //ACT
            client.Name = "Test";

            //ASSERT
            Assert.AreEqual("Test", client.Name); 
        
        }
    }
}