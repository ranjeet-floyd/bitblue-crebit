<?php
	include('../attach/header_clg.php');
?>
<div class="span-right">
	<div class="main-container">
		 <div class="post-header">
			<span> Add Student</span>
		 </div>
		 <div class="post-content">
			<div class="post-text">
				<form class="clg_ads" method="post" action="">
					<div class="box-left">
					     Enroll No.: <input   class="mando" type="text" name="clg_ads_enr">
					</div>
					<div class="box-right">
						 GR. No.: <input  class="mando"  type="text" name="clg_ads_grn">
					</div>
					<div class="box-left">
						 Student Name: <input  class="mando" type="text" name="clg_ads_name">
					</div>
					<div class="box-right">
						 Father Name: <input  class="mando" type="text" name="clg_ads_fname">
					</div>
					<div class="box-left">
						 Mother Name: <input  class="mando" type="text" name="clg_ads_mname">
					</div>
					<div class="box-right">
						 Sex: <select  class="mando" name="clg_ads_sex">
							 <option value="default">Select One</option>
							 <option value="male">Male</option>
							 <option value="female">Female</option>
						</select>
					</div>
					<div class="box-left">
						 Medium:<select  class="mando" name="clg_ads_mdm">
							 <option value="default">Select One</option>
							 <option value="English">English</option>
							 <option value="Marathi">Marathi</option>
						</select>
					</div>
					<div class="box-right">
						Course:<select  class="mando" name="clg_ads_cor">
							<option value="default">Select One</option>
							 <option value="B.A.">B.A.</option>
							 <option value="B.Com">B.Com</option>
							 <option value="B.Sc.">B.Sc.</option>
							 <option value="B.Tech">B.Tech</option>
							 <option value="MBA">MBA</option>
						</select>
					</div>
					<div class="box-left" onchange="div_count()">
					     Year:<select  class="mando" name="clg_ads_std">
							<option value="default">Select One</option>
							 <option value="first">First</option>
							 <option value="second">Second</option>
							 <option value="three">Three</option>
							 <option value="four">Four</option>
							 <option value="fifth">Fifth</option>
						</select>
					</div>
					<div class="box-right div-update-show">
						 Division:	 
					</div>
					<div class="box-left">
					     Date of Birth: <input   class="mando" type="date" name="clg_ads_dob">
					</div>
					<div class="box-right">
						 Birth Place: <input  type="text" name="clg_ads_bplc">
					</div>
					<div class="box-left">
					     Contact No.: <input  class="mando inputpn"  type="text" name="clg_ads_cont_num">
					</div>
					<div class="box-right">
						 Address: <input   class="mando" type="text" name="clg_ads_adrs">
					</div>
					<div class="box-left">
					     Documents: <input  type="text" name="clg_ads_docs">
					</div>
					<div class="box-right">
						 Religion: <input  type="text" name="clg_ads_relg">
					</div>
					<div class="box-left">
					     Caste: <input  type="text" name="clg_ads_cast">
					</div>
					<div class="box-right">
						 Sub-caste: <input  type="text" name="clg_ads_sub_cast">
					</div>
					<div class="box-left">
					     Nationality: <input  type="text" name="clg_ads_ntn">
					</div>
					<div class="box-right">
						 Last College: <input  type="text" name="clg_ads_lclg">
					</div>
					<div class="box-left">
					    Progress: <input  type="text" name="clg_ads_prog">
					</div>
					<div class="box-right">
					    test: <input  type="text" name="clg_ads_test">
					</div>
					<div class="box-right">
						 Adhar Card No.: <input  type="text" name="clg_ads_adhar">
					</div>
					<div class="box-left">
					    class_f_Addmission:<select name="clg_ads_class_Addm">
							<option value="default">Select One</option>
							 <option value="paying">Paying</option>
							 <option value="non-paying">Non-paying</option>
						</select>
					</div>
					<div class="box-right">
					    class_f_left:<select name="clg_ads_class_left">
							<option value="default">Select One</option>
							 <option value="paying">Paying</option>
							 <option value="non-paying">Non-paying</option>
						</select>
					</div>
					<div class="box-left">
					     Date of Addmission: <input  class="mando" type="date" name="clg_dt_Addm">
					</div>
					<div class="box-right">
					     Date of leaving : <input  class="mando" type="date" name="clg_dt_left">
					</div>
					<div class="box-left">
					    reason for leaving: <input  type="text" name="clg_ads_res">
					</div>
					<div class="box-right">
					    Tax-Status:<select name="clg_ads_tax_status">
							<option value="default">Select One</option>
							 <option value="paying">Paying</option>
							 <option value="non-paying">Non-paying</option>
						</select>
					</div>
					<div class="box-right">
							<button class="clg_ads_sub"><span>Submit</span></button>
					</div>
				</form>
			</div>
		 </div>
	</div>
</div>
	<?php
	include('../attach/footer_clg.php');
?>