# RabbitMQ Temel Kavramları

![RabbitMQ Logo](https://upload.wikimedia.org/wikipedia/commons/thumb/7/71/RabbitMQ_logo.svg/220px-RabbitMQ_logo.svg.png)

**RabbitMQ**, mesajları kuyruklar üzerinden yöneten bir mesaj aracıdır (message broker). Bu tür sistemlerde bazı temel kavramlar vardır: exchange, queue, publisher ve subscriber. Bu kavramların ne olduğunu ve ne amaçla kullanıldıklarını aşağıda açıklanmıştır.

## Exchange

**Nedir?**: Exchange, mesajların hangi kuyruğa yönlendirileceğine karar veren bileşendir. Mesajlar önce exchange'e gönderilir ve exchange, mesajı uygun kuyruklara yönlendirir.

**Neden Kullanılır?**: Exchange'ler mesajların dağıtımını kontrol eder. Farklı türlerde exchange'ler vardır:
- **Direct Exchange**: Belirli bir routing key'e sahip mesajları uygun kuyruğa yönlendirir.
- **Fanout Exchange**: Gelen mesajı tüm bağlı kuyruklara yönlendirir, routing key kullanmaz.
- **Topic Exchange**: Routing key'leri belirli desenlere göre eşleştirir ve uygun kuyruklara yönlendirir.
- **Headers Exchange**: Mesaj başlıklarına göre yönlendirme yapar, routing key kullanmaz.

## Queue (Kuyruk)

**Nedir?**: Kuyruk, mesajların depolandığı yerdir. Tüketiciler (subscribers) bu kuyruklardan mesaj alırlar.

**Neden Kullanılır?**: Kuyruklar, mesajları geçici olarak depolar ve tüketicilerin bu mesajları işlemelerini sağlar. Mesajlar kuyrukta sırayla bekler ve tüketiciler tarafından alındıkça kuyruktan çıkarılır.

## Publisher ve Subscriber

### Publisher

**Nedir?**: Publisher, mesajları exchange'e gönderen bileşendir. Hem exchange hem de Quee Publisherda oluşturulabilir. veya sadece Exchange publisher'da Quee Subscribe'da oluşturulabilir. farklı senaryolara göre bu şekilde kullanımla gerekebilir

**Amaç**: Veriyi (mesajları) oluşturur ve belirli bir exchange'e gönderir. Publisher, veri üreticisidir.

**Örnek**: Bir sipariş sistemi, yeni bir sipariş oluşturulduğunda bu sipariş bilgisini mesaj olarak exchange'e gönderir.

### Subscriber

**Nedir?**: Subscriber, kuyruktan mesajları alan ve işleyen bileşendir.

**Amaç**: Mesajları kuyruktan alır ve belirli bir işleme tabi tutar. Subscriber, veri tüketicisidir.

**Örnek**: Bir sipariş işleme sistemi, kuyruktan yeni sipariş mesajlarını alır ve bu siparişleri işler.

## RabbitMQ'nun Kullanım Amacı

RabbitMQ, dağıtılmış sistemlerde ve mikro hizmet mimarilerinde kullanılan mesajlaşma ihtiyaçlarını karşılar. İşte RabbitMQ'nun bazı kullanım amaçları:

- **İş Yükü Dağıtımı**: İş yükünü birden fazla tüketiciye dağıtarak sistemin ölçeklenebilirliğini artırır.
- **İletişim Sağlama**: Farklı servisler arasında mesajlaşma yoluyla iletişim kurmayı sağlar.
- **Güvenilir Mesajlaşma**: Mesajların kaybolmadan iletilmesini sağlar.
- **Asenkron İşleme**: İstemci ve sunucunun eşzamanlı olmasını gerektirmeden mesajları işlemeyi sağlar.

## Örnek Senaryo

Bir e-ticaret sitesinde, müşteri bir sipariş verdiğinde şu işlemler gerçekleşir:

- **Publisher (Sipariş Hizmeti)**: Yeni sipariş verildiğinde sipariş bilgilerini bir mesaj olarak RabbitMQ'ya gönderir.
- **Exchange (Direct Exchange)**: Sipariş mesajını uygun kuyruklara yönlendirir.
- **Queue (Sipariş Kuyruğu)**: Sipariş mesajını depolar.
- **Subscriber (Sipariş İşleme Hizmeti)**: Sipariş kuyruğundan mesajı alır ve siparişi işler.

Bu şekilde, siparişler bağımsız olarak işlenir ve sistemin diğer bölümlerini etkilemeden ölçeklenebilirlik ve esneklik sağlanır.

## Özet

- **Exchange**: Mesajları uygun kuyruğa yönlendiren bileşen.
- **Queue (Kuyruk)**: Mesajların depolandığı yer.
- **Publisher**: Mesajları exchange'e gönderen bileşen.
- **Subscriber**: Kuyruktan mesajları alıp işleyen bileşen.

RabbitMQ, bu bileşenler aracılığıyla mesajlaşmayı yönetir ve dağıtılmış sistemlerde verimli bir iletişim sağlar.
