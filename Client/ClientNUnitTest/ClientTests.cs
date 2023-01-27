namespace ClientNUnitTest
{
    public class Test
    {
        [SetUp]
        public void Setup()
        {
            //I setup kan vi ist�llet instansiera v�rat MathClass math variabel
        }


        //Demonstraion av ett test man kan k�ra
        [Test]
        public void TestExample()
        {
            // ARRANGE
            //var MathClass math = new MathClass(); 

            // ACT
            //int result = math.Add(1,1);

            // ASSERT
            //h�r anv�nder vi assert f�r att se att resultatet �r det vi f�rv�ntar oss att det ska vara, dvs att Add av 1+1 = 2
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