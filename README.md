# MicroserviceClass

Bu depo, Udemy üzerindeki **Microservice Architecture Öğrenelim** eğitimi kapsamında geliştirdiğim mikroservis tabanlı örnek projeleri içerir. Eğitim sürecinde mikroservis mimarisinin temel prensiplerini, servisler arası iletişimi, veri yönetimini ve dağıtık sistem yaklaşımını uygulamalı olarak öğrendim.

Projede iki ana çalışma yer almaktadır:

- **Todo uygulaması**: Mikroservis yaklaşımıyla geliştirilen küçük ölçekli bir örnek proje ve ocelot gateway mimarisi kullanılan uygulama
- **Mini e-ticaret uygulaması**: Daha kapsamlı bir senaryo üzerinde çoklu veritabanı ve Yarp gateway mimarisi kullanılan uygulama

Tüm bileşenler Docker ortamında test edilmiştir.

---

## Proje Hakkında

Bu proje, mikroservis mimarisini teoriden pratiğe taşıyan bir öğrenme deposudur. Amaç; farklı servisleri bağımsız şekilde tasarlamak, farklı veri tabanlarını aynı mimari içinde kullanmak ve servisler arası trafiği bir gateway üzerinden yönetmektir.

Projede özellikle şu konulara odaklanıldı:

- Mikroservis mimarisi
- Servislerin bağımsız çalışması
- API tabanlı servisler arası iletişim
- Veri ayrıştırma ve farklı veritabanı kullanımı
- Gateway katmanı ile merkezi yönlendirme
- Docker ile konteyner bazlı test ve çalıştırma

---

## Uygulamalar

### Todo Projesi
Küçük ölçekli bu uygulamada, mikroservis yaklaşımının ve ocelot'un temel mantığını kavramaya yönelik bir yapı oluşturuldu.  
Ayrıca bu proje **Angular** ile görselleştirilerek kullanıcı arayüzü tarafı da deneyimlendi.

### Mini E-Ticaret Projesi
Daha kapsamlı olan bu senaryoda, farklı servislerin birlikte çalıştığı ve Yarp gateway ile bir yapı kuruldu.  
Bu projede aşağıdaki teknolojiler kullanıldı:

- **PostgreSQL**
- **MongoDB**
- **MSSQL**
- **YARP Gateway**

Bu yapı sayesinde hem ilişkisel hem de doküman tabanlı veritabanlarının mikroservis mimarisi içinde nasıl kullanılabileceği uygulamalı olarak test edildi.

---

## Kullanılan Teknolojiler

- **C# / .NET**
- **Angular**
- **PostgreSQL**
- **MongoDB**
- **MSSQL**
- **YARP Gateway**
- **Docker**
- **minimal API**
- **Microservice Architecture**

---

## Öğrenilen Konular

Bu proje kapsamında aşağıdaki konularda pratik deneyim kazanıldı:

- Mikroservis tasarımı
- Servis ayrıştırma mantığı
- Farklı veritabanı tipleriyle çalışma
- Gateway kullanımı
- UI entegrasyonu
- Docker üzerinde sistemleri birlikte çalıştırma ve test etme

---

## Not

Bu repository, üretim ortamına yönelik ticari bir uygulamadan ziyade eğitim odaklı bir çalışma deposudur.  
Amaç; mikroservis mimarisini, modern backend teknolojilerini ve dağıtık sistem yaklaşımını uygulamalı şekilde öğrenmektir.

---

## Lisans

Bu proje eğitim amaçlı geliştirilmiştir.
