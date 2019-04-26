/*
 * Created by SharpDevelop.
 * User: Asus
 * Date: 23.04.2019
 * Time: 11:41
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Timers;
namespace car_race_atari
{
	class Program
	{
		static int tim=333;//zamanlaıcı başlangıçta 1 sn de bir jareket eder
		static int konum=3;//karakter başlangıç konumu 3.sütunda
		static int boy=17;//haritamızın boyu ise 20 satır
		static int en=6;//haritamız 4 sütundan oluşmakta
		static int[,] harita=new int[boy,en];//haritamızı int tipinde dizi değişkeninde tutuyoruz
		static int sayac=0;//sayac 0 dan başlar
		static double zaman=0;
		static double km=0;
		static double hiz;
		public static int botSayisi=10;//botsayisi 10 adet tanımladık
		private static Timer t=new Timer(tim);//timer intervalı olan tim adlı değişkeni ile tanımladık
		public static Random r=new Random();//Random referansını en tepeden tanımladık
		public static void yazdır()//haritayı yazdıran fonksiyonu
		{
			Console.Clear();//önce konsolu temizle
			for(int i=0;i<boy;i++){//satır satır
				for(int j=0;j<en;j++){//sütun sütun
				
					if(j==(en/2)||j==0)Console.Write("|");
					switch(harita[i,j]){//haritadaki sayılara göre karakterler ekrana basılacak
							case 0:Console.Write(" ");break;//0 sa boş
							case 1:Console.Write("v");break;//1 ise aşağı gider
							case 2:Console.Write("^");break;//2 ise yukarı gider
							case 3:Console.Write("X");break;//3 bizimkarakterimiz
							default:break;//varsayılan olarak bişey koymadık
					}
					if(j==en-1)Console.Write("|");
				}Console.WriteLine("");//Alt saıra geç
			}
			Console.WriteLine("\n"+(double)sayac*0.05+" km");//km göster
			Console.WriteLine("\n"+(int)18000/tim+" km/h");//hiz göster
			//Console.WriteLine("\n"+(int)zaman+" sn");//hiz göster
		}
		public static void botekle(){//bot eleyen fonsiyonumuz
			int n;
			int m;
			
			n=r.Next(5);//böyle tanımladık çünkü hemen dibimizde bitmesin(üretilmesin)
			m=r.Next(en);//hangi sütunda çıkacak
			bool flag=true;//bayrak tanımladık bu bayrak false olunca döngüyü kıracak
			while(flag)//döngü başlayacak
			{	
				if(harita[n,m]==0){//rastgele üretilen konum boşsa
					if(m<(int)en/2)//ve sütun 2 den küçükse
						harita[n,m]=1;//harita aşağı giden cisimolarak işaretle
					else
						harita[n,m]=2;//harita yukarı giden cisim olarak işaretle
					flag=false;//ve son olarak ta bayrağı çek
					}
				n=r.Next(5);//olmazsa yeni konumu atayalım
				m=r.Next(en);//bu da sütünu
			}	
		}
		public static void salto(char yon){//buda bizim sağa sola aksiyonumu anımlayack
			switch(yon){
					case '4':if(konum!=0 && harita[boy-1,konum-1]==0){//Eğer en soldaki sütunda değilse ve solundaki sütun boşsa dolu olabilir bot ile
						harita[boy-1,konum]=0;//önceki konumunu boşalt
						konum--;//konumu sola kaydır
						harita[boy-1,konum]=3;//yeni yerini güncelle
					}break;
				case '6':
					if(konum!=en-1 && harita[boy-1,konum+1]==0){//Eğer en sağdaki sütunda değilse ve sağındaki sütun boşsa dolu olabilir bot ile
						harita[boy-1,konum]=0;//öceki konumunu boşalt
						konum++;//sağa kaydır
						harita[boy-1,konum]=3;//yeni yerini güncelle
					}break;
					default:break;//varsayılan olarak birşey tanımlamadık
			}
		}
		public static void hareketi(){/// <summary>
		/// Botların göreceli olarak hareketlerini tanımlar
		/// ->Aşağı gidenler 1 saykılda bir 
		/// ->Yukarı doğru gidenler göreceli olarak 3saykılda bir 
		/// </summary>
			

			for(int b=boy-1;b>-1;b--){//en sondan başlıyoruz yoksa hepsi en sona itelenir
				for(int n=en-1;n>-1;n--){
					if(harita[b,n]==1)//
					{
						if(b+1<boy){
							if(harita[b+1,n]==3)//Önünde karakterimiz varsa çarpar ve oyun biter timer durur
							{
								t.Stop();
								Console.WriteLine("\nOyun bitti Kaza yaptın!");
							}
							
							harita[b,n]=0;
							
							harita[b+1,n]=1;
						}else if(b+1==boy){
							harita[b,n]=0;
							botekle();
						}
						
					}
					if(sayac%3==0){//yukarı doğru gidenler 3 sn de bir hareket eder
						if(harita[b,n]==2){
							if(b+1<boy){
								if(harita[b+1,n]==3)//Önünde karakterimiz varsa çarpar ve oyun biter timer durur
								{
									t.Stop();
									Console.WriteLine("\nOyun bitti Kaza yaptın!");
								}
								harita[b,n]=0;
								harita[b+1,n]=2;
							}else if(b+1==boy)
							{
								
								harita[b,n]=0;
								botekle();
							}
							
						}
					}
				}
			}
		}
		public static void ilkle(){/// <summary>
		/// Oyun bununla ilkenir matrisimiz bu fonsiyon vasıtasıyla doldurulur.
		/// 10 adet bot araç dolar
		/// </summary>

			for(int i=0;i<boy;i++){
				for(int j=0;j<en;j++){
					harita[i,j]=0;//haritamız başlangıçta bomboştur
				}
			}
			for(int i=0;i<botSayisi;i++)//botsayisina göre 10 tane bot dolar
			{
				botekle();	
			}
			harita[boy-1,konum]=3;//ve biz
		}
		private static void hareket(object o,ElapsedEventArgs a){
			hareketi();//botların hareketinden görevli fonksiyon
			yazdır();//renderlayan fonksiyon
			
			sayac++;//sayacımızı artır
			if(sayac%15==0){//her  15 de bir timerimizi artırarak oyna ritim katıyoruz
				if(tim>75)tim-=10;//timer 33 er olarak azalacak
				t.Interval=tim;//içsayacı güncelledik
			}
			//zaman+=(double)tim/1000;

		}
		public static void Main(string[] args)
		{
			ilkle();//oyunu ilkledir
			t.Elapsed+=new ElapsedEventHandler( Program.hareket);//timeri tanımladık
			t.Start();//timeri başlattık
			while(true){
				char b;//interrupt olarak b karakterini tanımladık
				b=Console.ReadKey(true).KeyChar;
				salto(b);//hareketimizden sorumlu fonksiyonumuz 
			}
			Console.ReadKey(true);
		}
	}
}