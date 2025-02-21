from PIL import Image
import os

def convert_jpg_to_png(directory):
    """
    Belirtilen klasördeki tüm .jpg ve .jpeg dosyalarını .png formatına dönüştürür.
    Dönüştürülen dosyalar aynı klasöre kaydedilir.
    """
    # Klasördeki tüm dosyaları döngü ile geziyoruz.
    for filename in os.listdir(directory):
        # Dosya uzantısı kontrolü (küçük harf ile kontrol ediyoruz)
        if filename.lower().endswith((".jpg", ".jpeg")):
            # Dosya yolu oluşturuluyor.
            img_path = os.path.join(directory, filename)
            # Görüntü açılıyor.
            with Image.open(img_path) as img:
                # Yeni dosya adı oluşturuluyor.
                png_filename = os.path.splitext(filename)[0] + ".png"
                png_path = os.path.join(directory, png_filename)
                # Görüntü PNG formatında kaydediliyor.
                img.save(png_path, "PNG")
                print(f"{filename} dosyası {png_filename} olarak dönüştürüldü.")

if __name__ == "__main__":
    # Dönüştürme işleminin yapılacağı klasörü belirleyin.
    directory = f"C://Users//islam//Desktop//codes//C#//wrath-engine//assets"  # Örnek klasör ismi, ihtiyacınıza göre değiştirebilirsiniz.
    convert_jpg_to_png(directory)
