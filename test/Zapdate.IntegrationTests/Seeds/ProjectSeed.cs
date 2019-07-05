using Zapdate.Core.Domain.Entities;
using Zapdate.Infrastructure.Data;
using Zapdate.Infrastructure.Identity;

namespace Zapdate.IntegrationTests.Seeds
{
    public class ProjectSeed : IDbSeed
    {
        private const string _privateKey = "{\"d\":\"HeJMIBeM09VmuNUpQdI7VPMbInEu9Xf5t6okLzQcaXCFH97od1aWrOjCkiZMRYwbwJZ9wz0QHINNRQHiWcEdJtXFQnRSg6Ig31wrbrTMitskqHkXeM5ks6aDTimwmd0jZ9S+37onk+43zaq2Sa7JOKV0cp4wcXMXqHy0ABL45zT1A8yK2V15KKHm5N1qNTXpfR+MbzSC+N6OrWspXs08mihBsB6tFoXE1/1M2XKpYvZLkPq4AL+2KNgMJrdeaPh0943JtLJP0oU87VpCSWJsKPV7TTDY09x8KoawhDQiDTpUiTYJXeDuMN7zpaGiUoTUmwUUFIBqPFDOZU/KsYEfOCefQoLsBQd2r1Olqg+KKw1sMKa84f3UlHqSN96ga/yKgt/662Lbit/g0ckH6XqPtQkAHpFkzpMBvTfASlJJhNEJsqAKpG7XAKf9ZvNRz86Vbp4yQvV3KS2miW4Oem/iRjhBuq9WSIJOVYAH04NK132GgCXJ2ndiVEqzupxUdJI6WWvnjSMbdgOxce4lBDtfTrSASYl4UGyn3wxQ2AJlvY0QqQiK2vq4OjXEtXUD4eV8j9UXsUNhJDfx8D5b6XM4c9UHkYsXhPjJn8imBnxeOFAIS1Vikkjy+vyiTx77R1GJBMt/OLqQcpYOFMAMEm0Fx64H+WKGxoYGtbDvPqTvDIE=\",\"dp\":\"eeiD4Q6DkFkqRrmZUf46gnd3GTSM1hmraiw5o2d9pCANOcRkk50hzjzSWKX5/2NMYunWXwnW079A5AN1izR/CsWWT2iXZVbUD8GTWJXhSJ6g6eWrSCvGawMc3aN28rAavzXV9Y0HI2qX0KUnD4wR6VNn5nzMTD8USx6w7WKy7K38uEsVbunr+OQiidAI8Y3/ck+g7JLLibPKbdvfWetJzlYZFIGe+31W52OW23B5psrS4bWWnrLXCIEfK6TR/s1oh+qziCxix+gmhCzCJd+MhcrhioTwiriOoENmPXaLoR5Wr9o5StSdqi7Sc0TL2664fJgcxEUt84EoZ3zhazst8w==\",\"dq\":\"ziGCOm+H76TiiUUQzqqi9t2OwMmH5Q/vwxPXAQMw/j/enrDy8eznwn414tH7RzjlhQrze/WMK7wEG3SoOZ7VCLvenaGL1J8kFUJHdAlyo1zuoVCuO1a3QoCKOmOcN/XPVwgzHdyWw0URfNBFfaGQLPqn+Wqnh5e7HoOaMdJw8XJ/JFIQOCFdT0QSw5qtj/Uy4W7FSV8j85CP5ZScDks6noUnqzayVZYyrybZo2rwQCStBljmX1p3CrUXWcuvPYsVPQxMflit9okhhyvGMUsCN3iBlPsHevqaUTx5Ed0YSRw1YchGe09OaZyz4zv4q2WD23mKr9qkPQgolJ/mHMyoAw==\",\"exponent\":\"AQAB\",\"inverseQ\":\"IFqy44C4fYFFTWQN4Bd8xnGCY9Rx41ycDgyDmgM7LgwN7D3DNILAvfHYsnr7bd+Ny2a9n/udzmD4e5Ma8wHqZshK9JUfWdaXnJZX1pOBC9YPa3OhpMzqC56fjnWcxGBFtWoIGHPdldd63mmRTb9zDZeULqkBudp7wPK6ugjfYdzcarMcuSM1ljti2IW9mhVvifJMr2CaxDu43In+1XXTr4QZG9oEqq/yEcuRrslxOycMNheRU3SjAIqjmLY+vLNYrzbb4hPpOPcxNftRqebgrXPny2BvGtc3abQAFsy2iEXNk6s1S0KXAjUc4IJO+64uYCuJAjk6qqrcp/zJ825N5A==\",\"modulus\":\"qnuWhAEAQPDnW+Wpg9JMdftd5S1QA7hQ7Yusa4FUFxtsCslHF4LO3+X15z1KbO7qLVTSOO6amQjGbLpH7NLfX/naCGoBPzZLnvQeMTVgQn0jBaeTS7fsFDdrbK33haeECGHXz1q5t8AVTRHqWT/i7cliOuOLZsOXE7UMDP20SFTGFABTkDri5jf3l/wOrUi01iED5OxwQOzqKTjXk2aVM3NtpXIIUP/lkM4w/0aoxYT1dzn16mwv98JtyeIqFHnki9PQ/cWgjalY1nG66Uk3KWVan0pLsVJc27IpmiGnm1+X962yAPFLRXZ5U+funy3L8OJi+gn0hIZMyzDYzw9Y/jgchjTOV0ksADpi8RffsqhlTDT0d1aRqrN1lmNqfDPUmTR/AXDtbjxffagz2BTo6s90U7NKC2GE3IgqLBDOtKWi5M0sE2QgHUyFYAD83o1T00tZRvx5MVoH0pONCU+c+P/3e/6dsbRSSy5SoReYUyzztGqLgynYeDQN4C2UYuODYEx0c5avB88kqwPoauCCCbXgzwOrvb/GGGGnccUVl7BXY5+qwCNnbIGV3JgVRd5+cCFbd5l85mFSducekVbGPhtgMahh3JqIqqBXRhdfoFxnm8/f3P7riLe2fYmn9ApHCMtTte9Dvr7snwZJ0yh+faDUKp2KoyJJlBJbGlA4XC0=\",\"p\":\"0u7MMQfIkiDg7e7fq5gU0lFRZlhL0t5it38tcytgtTnpPEPRvk2j5sRmF/f2JDjTZpCopkQSqi9iB79zhIume4pFOsJ3oPzJGlYWJITVwS8SQkQ0FOQ+9L5aC3OVtqJr9Yqb5Hi+DuT3eNv0KdgXxek0UYq6XznkXyfpGhs9XymDxtHlnStuRiSIpb2+HPzdLKYLBKC5HHTYeroTYfHmQqB2KPZrYGNleTwGnfJeR3hL1iBLnlU2TUe92dAsQOgd78Kndb47CFpyspnbj656oDDtuiUnMQdGVHvx3djEKayON9+sjOsuAxJeItRP/9rvszmZqXzvuQ3xT5e0oKUw6w==\",\"q\":\"zuhUtQL+PHy1h+Kl/JYtWuiGJrtZ+7W5vcY4yTK/kshWmXBqOIfzy7fvaSeA55HLRXn2P5Pgp0hIBGT4T+uLE6q5vUJJ5uTxz9JA7nUcwVSX86uCsBd5EBQwDOoyviH91MQp4iUiKY2ZRZFWZZZGoCIVkrgLFNo3jWkOQA/yBoOsmwGNBih6LY773rmYydvyVY2HKoAZ64Lnve1Ftp8PIzx/Ff5qB8H05LhtSt05YwCs0GlhkqIrmOV1qH1I8AgKeihyP/xo35URukSQ+9IzrfAv61jYfTHmDQE8fToCqsHqEwHZCEq5PDG5FhH7HLCkTjxF2D1GvF6sN5mw0uyhRw==\"}";
        private const string _publicKey = "{\"exponent\":\"AQAB\",\"modulus\":\"qnuWhAEAQPDnW+Wpg9JMdftd5S1QA7hQ7Yusa4FUFxtsCslHF4LO3+X15z1KbO7qLVTSOO6amQjGbLpH7NLfX/naCGoBPzZLnvQeMTVgQn0jBaeTS7fsFDdrbK33haeECGHXz1q5t8AVTRHqWT/i7cliOuOLZsOXE7UMDP20SFTGFABTkDri5jf3l/wOrUi01iED5OxwQOzqKTjXk2aVM3NtpXIIUP/lkM4w/0aoxYT1dzn16mwv98JtyeIqFHnki9PQ/cWgjalY1nG66Uk3KWVan0pLsVJc27IpmiGnm1+X962yAPFLRXZ5U+funy3L8OJi+gn0hIZMyzDYzw9Y/jgchjTOV0ksADpi8RffsqhlTDT0d1aRqrN1lmNqfDPUmTR/AXDtbjxffagz2BTo6s90U7NKC2GE3IgqLBDOtKWi5M0sE2QgHUyFYAD83o1T00tZRvx5MVoH0pONCU+c+P/3e/6dsbRSSy5SoReYUyzztGqLgynYeDQN4C2UYuODYEx0c5avB88kqwPoauCCCbXgzwOrvb/GGGGnccUVl7BXY5+qwCNnbIGV3JgVRd5+cCFbd5l85mFSducekVbGPhtgMahh3JqIqqBXRhdfoFxnm8/f3P7riLe2fYmn9ApHCMtTte9Dvr7snwZJ0yh+faDUKp2KoyJJlBJbGlA4XC0=\"}";

        public void PopulateTestData(AppIdentityDbContext dbContext)
        {
            new AccountSeed().PopulateTestData(dbContext);
        }

        public void PopulateTestData(AppDbContext dbContext)
        {
            dbContext.Add(new Project("Test Project", new AsymmetricKey(_publicKey, _privateKey)));

            dbContext.Add(new StoredFile("13879d586271db46f545166cf67ef5aa585fe99b6121b7324467f9c751a8d93b", 1024, 827));
            dbContext.Add(new StoredFile("936a185caaa266bb9cbe981e9e05cb78cd732b0b3280eb944412bb6f8f8f07af", 2047, 1002));
            dbContext.SaveChanges();
        }
    }
}
