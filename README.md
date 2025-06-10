# Siparis-Analizi-Yapan-E-Ticaret-Sitesi

AIStyle, kullanıcıların önceki alışveriş alışkanlıklarını analiz ederek benzer ürünleri öneren, kişiselleştirilmiş alışveriş deneyimi sunan bir web tabanlı e-ticaret sistemidir.
Uygulama, geniş kapsamlı ve kategorilere ayrılmış 14.000’i aşkın ürün verisiyle çalışmakta olup, kullanıcının daha önce satın aldığı ürünleri analiz ederek bu verilerden anlamlı sonuçlar çıkarmayı ve kullanıcılara kişisel öneriler yapmayı hedeflemektedir. 

Myntra veri setini ön işleme aşamasında, Python’un Pandas ve NumPy kütüphanelerini kullanarak eksik ve hatalı verileri temizlemiştik. Bu süreç sonucunda 14.041 üründen oluşan tutarlı ve güvenilir bir veri seti elde etmiştik.
Daha sonra, veriyi Türkçe bir e-ticaret platformuna uyarlamak amacıyla, Google Translate API’si ile ürün bilgilerini parça parça çevirerek dil bariyerini aşmıştık. API kaynaklı sorunları bu yöntemle çözerek, sütun adları ve ürün açıklamalarını yerel kullanıcıların rahatlıkla anlayabileceği şekilde dönüştürmüştük.

Backend tarafında ASP.NET Core ile MVC ve katmanlı mimari kullanarak kullanıcı yönetimi, ürün listesi, sipariş işlemleri ve öneri sistemine ait yapıları geliştirmiştik.   
Veritabanı erişimi için Entity Framework Core ile PostgreSQL bağlantısı kurduk ve veri erişim katmanını oluşturmuştuk. 

Projede kullanılan öneri sistemi, kullanıcıların geçmişte satın aldığı ürünlerin isim, açıklama ve renk özelliklerini analiz ederek benzer içeriklere sahip yeni ürünler önerir. 
ML.NET kullanılarak geliştirilen bu sistem, metinleri TF-IDF yardımıyla sayısal verilere dönüştürerek ürünler arasında kosinüs benzerliği hesaplar ve her kullanıcıya özel öneriler sunar. Böylece kullanıcıya, ilgisini çekebilecek ürünlerle daha kişiselleştirilmiş bir alışveriş deneyimi sağlanır.

Frontend geliştirme sürecinde, kullanıcı deneyimini artırmak ve tasarım süresini kısaltmak amacıyla modern ve bir HTML/CSS şablonu tercih ettik. 
Template’i projemize uyarlarken,  MVC yapısını kullandık. 
Her sayfa için ayrı viewlar ve controllerlar oluşturduk, her sayfanın yönetimini dinamik bir şekilde yapmamızı sağladı. 


