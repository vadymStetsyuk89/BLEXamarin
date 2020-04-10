using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyClasses;
using System;

namespace MyClassesTest
{
    [TestClass]
    public class FileProcessTest
    {
        private const string BAD_FILE_PATH = @"C:\Windows\BAD_FILE.exe";

        [TestMethod]
        public void FileNameDoesExists()
        {
            FileProcess fileProcess = new FileProcess();

            bool result = fileProcess.FileExists(@"C:\Windows\Regedit.exe");

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void FileNameDoesNotExists()
        {
            FileProcess fileProcess = new FileProcess();

            bool result = fileProcess.FileExists(BAD_FILE_PATH);

            Assert.IsFalse(result);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void FileNameNullOrEmpty_ThrowsArgumentNullException()
        {
            FileProcess fileProcess = new FileProcess();

            fileProcess.FileExists(string.Empty);
        }

        [TestMethod]
        public void FileNameNullOrEmpty_ThrowsArgumentNullException_UsingTryCatch()
        {
            FileProcess fileProcess = new FileProcess();

            try
            {
                fileProcess.FileExists("");
            }
            catch (ArgumentNullException)
            {
                /// The test was success
                return;
            }

            Assert.Fail("Call to FileExists did NOT throw an ArgumentNullException");
        }
    }
}
