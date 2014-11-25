<?php
include('../attach/header_sch.php');
function getExtension($str) {$i=strrpos($str,".");if(!$i){return"";}$l=strlen($str)-$i;$ext=substr($str,$i+1,$l);return $ext;}
$formats = array("jpg", "png", "gif", "bmp", "jpeg", "PNG", "JPG", "JPEG", "GIF", "BMP");
if(isset($_POST) and $_SERVER['REQUEST_METHOD'] == "POST"){
 $name = $_FILES['himg']['name'];
 $size = $_FILES['himg']['size'];
 $tmp  = $_FILES['himg']['tmp_name'];
 if(strlen($name)){
  $ext = getExtension($name);
  if(in_array($ext,$formats)){
   if($size<(1024*1024)){
    $imgn = time().".".$ext;
	$imgn="uploads/".$imgn;
	if(move_uploaded_file($tmp,$imgn)){
		echo"Logo Updated";
		include('SimpleImage.php');
	   $image = new SimpleImage();
	   $image->load($imgn);
	   $image->resize(60,60);
	   $image->save($imgn);
	   mysqli_query($con,"UPDATE info SET Logo='".$imgn."' where key_p='007'");
	}else{
     echo "Uploading Failed.";
    }
   }else{
    echo "Image File Size Max 1 MB";
   }
  }else{
   echo "Invalid Image file format.";
  }
 }else{
  echo "Please select an image.";
 }
}
sleep(4);
include('../attach/footer_sch.php');
?>