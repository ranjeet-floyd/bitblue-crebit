<?php
include('../../db.php');

if($_POST['count']=='1'){
	parse_str($_POST['adm_upd_show'], $set);
	$id=mysqli_fetch_assoc(mysqli_query($con,"SELECT * from user_clg	
		where Gr_num='".$set['adm_upd_show_grn']."'
		")
	);
	$det=mysqli_fetch_assoc(mysqli_query($con,"SELECT * from clg_details	
		where Gr_num='".$set['adm_upd_show_grn']."'
		")
	);
	$string='<br/><br/><br/><br/><span class="adm_upd_grn_span" style="display:none">'.$set['adm_upd_show_grn'].'</span>';
}
else{
	$id=mysqli_fetch_assoc(mysqli_query($con,"SELECT * from user_clg	
		where Gr_num='".$_POST['adm_upd_show']."'
		")
	);
	$det=mysqli_fetch_assoc(mysqli_query($con,"SELECT * from clg_details	
		where Gr_num='".$_POST['adm_upd_show']."'
		")
	);
		$string='<br/><br/><br/><br/><span class="adm_upd_grn_span" style="display:none">'.$_POST['adm_upd_show'].'</span>';
}
//print_r($set['adm_upd_show_grn']);
$arr['id']=$id;
$arr['det']=$det;
if(!$id){
	?>
	<p>No Student for this GR/SR number.</p>
	<?php
	exit;
	}

$string=$string.'<form class="adm_upd_std_form">
<div class="box-left">
	 Name <input class="mando" value="'.$id['Name'].'" type="text" name="adm_upd_name">
</div>
<div class="box-right">
	Gr. No. <input class="mando" value="'.$id['Gr_num'].'" type="text" name="adm_upd_grn">
</div>
<div class="box-left">
	 Medium:<select class="mando" name="adm_upd_mdm">
		 <option value="English">English</option>
		 <option value="Marathi">Marathi</option>
	</select>
</div>
<div class="box-right">
						Course:<select class="mando" name="adm_upd_cor">
							 <option value="B.A.">B.A.</option>
							 <option value="B.Com">B.Com</option>
							 <option value="B.Sc.">B.Sc.</option>
							 <option value="B.Tech">B.Tech</option>
							 <option value="MBA">MBA</option>
						</select>
					</div>
<div class="box-left" >
	 Standard: <select name="adm_upd_std" class="mando">
							 <option value="first">First</option>
							 <option value="second">Second</option>
							 <option value="three">Three</option>
							 <option value="four">Four</option>
						</select>
</div>
<div class="box-right">
	 Division: <input  class="mando" value="'.$id['Section'].'" type="text" name="adm_upd_div">
</div>

<div class="box-left">
	 Enroll Number: <input  class="mando" class="mando"  value="'.$det['Enroll'].'" type="text" name="adm_upd_enroll">
</div>
<div class="box-right">
	 Father Name: <input  class="mando"  value="'.$det['f_name'].'" type="text" name="adm_upd_fname">
</div>
<div class="box-left">
	 Mother Name: <input class="mando"   value="'.$det['m_name'].'" type="text" name="adm_upd_mname">
</div>
<div class="box-right">
	Sex: <select class="mando" name="adm_upd_sex">
							 <option value="male">Male</option>
							 <option value="female">Female</option>
						</select>
</div>
<div class="box-left">
	 DOB: <input class="mando"   value="'.$det['DOB'].'" type="date" name="adm_upd_dob">
</div>
<div class="box-right">
	 Birth Place: <input  value="'.$det['birth_place'].'" type="text" name="adm_upd_bplace">
</div>
<div class="box-left">
	 Contact No.: <input class="mando inputpn"  value="'.$det['cont_num'].'" type="text" name="adm_upd_cont">
</div>
<div class="box-right">
	 Address: <input class="mando"  value="'.$det['address'].'" type="text" name="adm_upd_adrs">
</div>
<div class="box-left">
	 Documents: <input  value="'.$det['docs'].'" type="text" name="adm_upd_docs">
</div>
<div class="box-right">
	 Religion:<input  value="'.$det['religion'].'" type="text" name="adm_upd_rel">
</div>
<div class="box-left">
	 Caste: <input  value="'.$det['caste'].'" type="text" name="adm_upd_cast">
</div>
<div class="box-right">
	 Sub Caste:<input  value="'.$det['sub_caste'].'" type="text" name="adm_upd_scast">
</div>
<div class="box-left">
	 Nationality: <input  value="'.$det['nationality'].'" type="text" name="adm_upd_nati">
</div>
<div class="box-right">
	 Last School:<input  value="'.$det['last_school'].'" type="text" name="adm_upd_lsch">
</div>
<div class="box-left">
	 Progress: <input  value="'.$det['progress'].'" type="text" name="adm_upd_prog">
</div>
<div class="box-right">
	 Adhar Number:<input  value="'.$det['last_school'].'" type="text" name="adm_upd_adhr">
</div>
<div class="box-left">
	Tax-Status <select name="adm_upd_tsta">
							 <option value="paying">Paying</option>
							 <option value="non-paying">Non-Paying</option>
						</select>
</div>
<div class="box-right">
	<button class="adm_upd_sub"><span>Update</span></button>
</div>
</form>';
$arr['status']="Suceed";
$arr['data']=$string;
echo json_encode($arr);
exit;
?>
